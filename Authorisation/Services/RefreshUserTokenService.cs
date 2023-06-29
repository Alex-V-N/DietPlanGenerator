using Authorisation.Configuration;
using Authorisation.DTOs;
using FitCookieAI_ApplicationService.DTOs.UserRelated;
using FitCookieAI_Data.Entities.UserRelated;
using FitCookieAI_Repository.Implementations.Base;
using FitCookieAI_Repository.Implementations.UserRelated;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authorisation.Services
{
	public class RefreshUserTokenService
	{
		private readonly JwtConfig? _jwtConfig;
		private readonly TokenValidationParameters? _tokenValidationParams;

		public RefreshUserTokenService(IOptionsMonitor<JwtConfig> optionsMonitor, TokenValidationParameters tokenValidationParams)
		{
			_jwtConfig = optionsMonitor.CurrentValue;
			_tokenValidationParams = tokenValidationParams;
		}

		public async Task<JwtResult> RefreshUserToken(TokenRequestDTO tokenRequest)
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				if (tokenRequest != null)
				{
					var result = await VerifyAndGenerateUserToken(tokenRequest);

					if (result == null)
					{
						unitOfWork.Commit();

						return new JwtResult()
						{
							JwtErrors = new List<string>() {
							"Invalid tokens"
							},

							JwtSuccess = false
						};
					}

					unitOfWork.Commit();

					return result;
				}

				unitOfWork.Commit();

				return new JwtResult()
				{
					JwtErrors = new List<string>() {
					"Invalid payload"
					},

					JwtSuccess = false
				};
			}
		}

		public async Task<JwtResult> GenerateUserJwtToken(UserDTO user)
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				await ClearUsedUserTokens(user);

				JwtSecurityTokenHandler jwtTokenHandler = new JwtSecurityTokenHandler();

				RefreshUserTokensRepository refreshTokensRepo = new RefreshUserTokensRepository(unitOfWork);
				List<RefreshUserToken> tokens = await refreshTokensRepo.GetAll(t => t.IsUsed == true);

				foreach (var t in tokens)
				{
					t.IsRevorked = true;
					await refreshTokensRepo.Save(t);
				}

				var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

				SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
				{
					Subject = new ClaimsIdentity(new[]
					{
					new Claim("Id", user.Id.ToString()),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
				}),
					Expires = DateTime.UtcNow.AddMinutes(30),
					SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
				};

				var token = jwtTokenHandler.CreateToken(tokenDescriptor);
				var jwtToken = jwtTokenHandler.WriteToken(token);

				RefreshUserTokenDTO refreshTokenDTO = new RefreshUserTokenDTO()
				{
					JwtId = token.Id,
					IsUsed = false,
					IsRevorked = false,
					UserId = user.Id,
					AddedDate = DateTime.UtcNow,
					ExpiryDate = DateTime.UtcNow.AddMinutes(30),
					Token = RandomString(35) + Guid.NewGuid()
				};

				RefreshUserToken refreshToken = new RefreshUserToken
				{
					JwtId = refreshTokenDTO.JwtId,
					IsUsed = refreshTokenDTO.IsUsed,
					IsRevorked = refreshTokenDTO.IsRevorked,
					UserId = user.Id,
					AddedDate = refreshTokenDTO.AddedDate,
					ExpiryDate = refreshTokenDTO.ExpiryDate,
					Token = refreshTokenDTO.Token
				};

				await refreshTokensRepo.Save(refreshToken);

				unitOfWork.Commit();

				return new JwtResult()
				{
					Token = jwtToken,
					JwtSuccess = true,
					RefreshToken = refreshToken.Token
				};
			}
		}

		public async Task<JwtResult?> VerifyAndGenerateUserToken(TokenRequestDTO tokenRequest)
		{
			JwtSecurityTokenHandler jwtTokenHandler = new JwtSecurityTokenHandler();

			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				RefreshUserTokensRepository refreshTokensRepo = new RefreshUserTokensRepository(unitOfWork);

				UsersRepository usersRepo = new UsersRepository(unitOfWork);
				User user = new User();
				UserDTO userDTO = new UserDTO(); ;

				try
				{
					// Validation 1 - Validation JWT token format
					var tokenInVerification = jwtTokenHandler
						.ValidateToken(tokenRequest.Token, _tokenValidationParams, out var validatedToken);

					// Validation 2 - Validate encryption alg
					if (validatedToken is JwtSecurityToken jwtSecurityToken)
					{
						var result = jwtSecurityToken
							.Header
							.Alg
							.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

						if (result == false)
						{
							return null;
						}
					}

					// Validation 3 - validate expiry date
					var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

					var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

					var storedToken = await refreshTokensRepo.GetFirstOrDefault(x => x.Token == tokenRequest.RefreshToken);

					if (expiryDate >= DateTime.UtcNow)
					{
						unitOfWork.Commit();

						if (storedToken != null)
						{
							storedToken.IsUsed = true;
							await refreshTokensRepo.Save(storedToken);

							// Generate a new token
							user = await usersRepo.GetById(storedToken.UserId);

							userDTO = new UserDTO();

							if (user != null)
							{
								userDTO.Id = user.Id;
								userDTO.Email = user.Email;
								userDTO.Password = user.Password;
								userDTO.FirstName = user.FirstName;
								userDTO.LastName = user.LastName;
								userDTO.DOB = user.DOB;
							}

							return await GenerateUserJwtToken(userDTO);
						}
						else
						{
							unitOfWork.Dispose();

							return new JwtResult()
							{
								JwtSuccess = false,
								JwtErrors = new List<string>() {
								"Token does not exist"
								}
							};
						}
					}

					// validation 4 - validate existence of the token

					if (storedToken == null)
					{
						unitOfWork.Dispose();

						return new JwtResult()
						{
							JwtSuccess = false,
							JwtErrors = new List<string>() {
							"Token does not exist"
							}
						};
					}

					// Validation 5 - validate if used
					if (storedToken.IsUsed)
					{
						storedToken.IsRevorked = true;

						unitOfWork.Dispose();

						return new JwtResult()
						{
							JwtSuccess = false,
							JwtErrors = new List<string>() {
							"Token has been used"
							}
						};
					}

					// Validation 6 - validate if revoked
					if (storedToken.IsRevorked)
					{
						unitOfWork.Dispose();

						return new JwtResult()
						{
							JwtSuccess = false,
							JwtErrors = new List<string>() {
							"Token has been revoked"
							}
						};
					}

					// Validation 7 - validate the id
					var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

					if (storedToken.JwtId != jti)
					{
						unitOfWork.Dispose();

						return new JwtResult()
						{
							JwtSuccess = false,
							JwtErrors = new List<string>() {
							"Token doesn't match"
							}
						};
					}

					unitOfWork.Dispose();

					return new JwtResult()
					{
						JwtSuccess = false,
						JwtErrors = new List<string>() {
							"End error!"
						}
					};
				}
				catch (Exception ex)
				{
					if (ex.Message.Contains("Lifetime validation failed. The token is expired."))
					{

						unitOfWork.Dispose();

						return new JwtResult()
						{
							JwtSuccess = false,
							JwtErrors = new List<string>() {
							"Token has expired please re-login"
							}
						};

					}
					else
					{
						unitOfWork.Dispose();

						return new JwtResult()
						{
							JwtSuccess = false,
							JwtErrors = new List<string>() {
							"Something went wrong."
							}
						};
					}
				}

			}
		}
		private async Task ClearUsedUserTokens(UserDTO user)
		{
			using (MyUnitOfWork uow = new MyUnitOfWork())
			{
				uow.BeginTransaction();

				RefreshUserTokensRepository refreshUserTokensRepository = new RefreshUserTokensRepository(uow);
				List<RefreshUserToken> refreshUserTokens = await refreshUserTokensRepository
					.GetAll(t => t.IsUsed == true && t.IsRevorked == true && t.UserId == user.Id
					|| t.ExpiryDate < DateTime.Now);

				foreach (var token in refreshUserTokens)
				{
					await refreshUserTokensRepository.Delete(token);
				}

				uow.Commit();
			}
		}
		public async Task ClearAllUserTokens(UserDTO user)
		{
			using (MyUnitOfWork uow = new MyUnitOfWork())
			{
				uow.BeginTransaction();

				RefreshUserTokensRepository refreshUserTokensRepository = new RefreshUserTokensRepository(uow);
				List<RefreshUserToken> refreshUserTokens = await refreshUserTokensRepository.GetAll(t => t.UserId == user.Id);

				foreach (var token in refreshUserTokens)
				{
					await refreshUserTokensRepository.Delete(token);
				}

				uow.Commit();
			}
		}
		public async Task<int> GetUserTokensCount(UserDTO user)
		{
			using (MyUnitOfWork uow = new MyUnitOfWork())
			{
				uow.BeginTransaction();

				RefreshUserTokensRepository refreshUserTokensRepository = new RefreshUserTokensRepository(uow);
				List<RefreshUserToken> refreshUserTokens = await refreshUserTokensRepository.GetAll(t => t.UserId == user.Id);

				uow.Commit();

				return refreshUserTokens.Count;
			}
		}
		private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
		{
			DateTime dateTimeVal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
				DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond, DateTimeKind.Utc);
			dateTimeVal = dateTimeVal.AddMinutes(unixTimeStamp).ToUniversalTime();

			return dateTimeVal;
		}

		private string RandomString(int length)
		{
			Random random = new Random();
			string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
				.Select(x => x[random.Next(x.Length)]).ToArray());
		}
	}
}

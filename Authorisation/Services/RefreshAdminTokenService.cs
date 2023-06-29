using Authorisation.Configuration;
using Authorisation.DTOs;
using FitCookieAI_ApplicationService.DTOs.AdminRelated;
using FitCookieAI_Data.Entities.AdminRelated;
using FitCookieAI_Repository.Implementations.AdminRelated;
using FitCookieAI_Repository.Implementations.Base;
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
	public class RefreshAdminTokenService
	{
		private readonly JwtConfig _jwtConfig;
		private readonly TokenValidationParameters _tokenValidationParams;

		public RefreshAdminTokenService(IOptionsMonitor<JwtConfig> optionsMonitor, TokenValidationParameters tokenValidationParams)
		{
			_jwtConfig = optionsMonitor.CurrentValue;
			_tokenValidationParams = tokenValidationParams;
		}

		public async Task<JwtResult> RefreshAdminToken(TokenRequestDTO tokenRequest)
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				if (tokenRequest != null)
				{
					var result = await VerifyAndGenerateAdminToken(tokenRequest);

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
		public async Task<JwtResult> GenerateAdminJwtToken(AdminDTO admin)
		{
			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				await ClearUsedAdminTokens(admin);

				JwtSecurityTokenHandler jwtTokenHandler = new JwtSecurityTokenHandler();

				RefreshAdminTokensRepository refreshTokensRepo = new RefreshAdminTokensRepository(unitOfWork);
				List<RefreshAdminToken> tokens = await refreshTokensRepo.GetAll(t => t.IsUsed == true);

				foreach (var t in tokens)
				{
					t.IsRevorked = true;
					await refreshTokensRepo.Save(t);
				}

				var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret); //_jwtConfig.Secret

				SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
				{
					Subject = new ClaimsIdentity(new[]
					{
					new Claim("Id", admin.Id.ToString()),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
				}),
					Expires = DateTime.UtcNow.AddMinutes(30),
					SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
				};

				var token = jwtTokenHandler.CreateToken(tokenDescriptor);
				var jwtToken = jwtTokenHandler.WriteToken(token);

				RefreshAdminTokenDTO refreshAdminTokenDTO = new RefreshAdminTokenDTO()
				{
					JwtId = token.Id,
					IsUsed = false,
					IsRevorked = false,
					AdminId = admin.Id,
					AddedDate = DateTime.UtcNow,
					ExpiryDate = DateTime.UtcNow.AddMinutes(30),
					Token = RandomString(35) + Guid.NewGuid()
				};

				RefreshAdminTokensRepository refreshAdminTokensRepo = new RefreshAdminTokensRepository(unitOfWork);

				RefreshAdminToken refreshAdminToken = new RefreshAdminToken
				{
					JwtId = refreshAdminTokenDTO.JwtId,
					IsUsed = refreshAdminTokenDTO.IsUsed,
					IsRevorked = refreshAdminTokenDTO.IsRevorked,
					AdminId = admin.Id,
					AddedDate = refreshAdminTokenDTO.AddedDate,
					ExpiryDate = refreshAdminTokenDTO.ExpiryDate,
					Token = refreshAdminTokenDTO.Token
				};

				await refreshAdminTokensRepo.Save(refreshAdminToken);

				unitOfWork.Commit();

				return new JwtResult()
				{
					Token = jwtToken,
					JwtSuccess = true,
					RefreshToken = refreshAdminToken.Token
				};
			}
		}
		public async Task<JwtResult?> VerifyAndGenerateAdminToken(TokenRequestDTO tokenRequest)
		{
			JwtSecurityTokenHandler jwtTokenHandler = new JwtSecurityTokenHandler();

			using (MyUnitOfWork unitOfWork = new MyUnitOfWork())
			{
				unitOfWork.BeginTransaction();

				RefreshAdminTokensRepository refreshAdminTokensRepo = new RefreshAdminTokensRepository(unitOfWork);

				AdminsRepository adminsRepo = new AdminsRepository(unitOfWork);

				Admin admin = new Admin();
				AdminDTO adminDTO = new AdminDTO();

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

					var storedToken = await refreshAdminTokensRepo.GetFirstOrDefault(x => x.Token == tokenRequest.RefreshToken);

					if (expiryDate > DateTime.UtcNow)
					{

						if (storedToken != null)
						{
							// update current token 
							storedToken.IsUsed = true;
							await refreshAdminTokensRepo.Save(storedToken);

							// Generate a new token
							admin = await adminsRepo.GetById(storedToken.AdminId);

							adminDTO = new AdminDTO();

							if (admin != null)
							{
								adminDTO.Id = admin.Id;
								adminDTO.Email = admin.Email;
								adminDTO.Password = admin.Password;
								adminDTO.FirstName = admin.FirstName;
								adminDTO.LastName = admin.LastName;
							}

							unitOfWork.Commit();

							return await GenerateAdminJwtToken(adminDTO);
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
						unitOfWork.Dispose();

						storedToken.IsRevorked = true;

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
		private async Task ClearUsedAdminTokens(AdminDTO admin)
		{
			using (MyUnitOfWork uow = new MyUnitOfWork())
			{
				uow.BeginTransaction();

				RefreshAdminTokensRepository refreshAdminTokensRepository = new RefreshAdminTokensRepository(uow);
				List<RefreshAdminToken> refreshAdminTokens = await refreshAdminTokensRepository
					.GetAll(t => t.IsUsed == true && t.IsRevorked == true && t.AdminId == admin.Id
					|| t.ExpiryDate < DateTime.Now);

				foreach (var token in refreshAdminTokens)
				{
					await refreshAdminTokensRepository.Delete(token);
				}

				uow.Commit();
			}
		}
		public async Task<int> GetAdminTokensCount(AdminDTO admin)
		{
			using (MyUnitOfWork uow = new MyUnitOfWork())
			{
				uow.BeginTransaction();

				RefreshAdminTokensRepository refreshAdminTokensRepository = new RefreshAdminTokensRepository(uow);
				List<RefreshAdminToken> refreshAdminTokens = await refreshAdminTokensRepository.GetAll(t => t.AdminId == admin.Id);

				uow.Commit();

				return refreshAdminTokens.Count;
			}
		}
		public async Task ClearAllAdminTokens(AdminDTO admin)
		{
			using (MyUnitOfWork uow = new MyUnitOfWork())
			{
				uow.BeginTransaction();

				RefreshAdminTokensRepository refreshAdminTokensRepository = new RefreshAdminTokensRepository(uow);
				List<RefreshAdminToken> refreshAdminTokens = await refreshAdminTokensRepository.GetAll(t => t.AdminId == admin.Id);

				foreach (var token in refreshAdminTokens)
				{
					await refreshAdminTokensRepository.Delete(token);
				}

				uow.Commit();
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

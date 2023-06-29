using Authorisation.Configuration;
using Authorisation.DTOs;
using Authorisation.Services;
using FitCookieAI_API.Responses;
using FitCookieAI_ApplicationService.DTOs.UserRelated;
using FitCookieAI_ApplicationService.Implementations.UserRelated;
using GlobalVariables.Encription;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FitCookieAI_API.Controllers.UserRelated
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly RefreshUserTokenService _usersAuthentication;
        private readonly RefreshAdminTokenService _adminsAuthentication;
        private readonly TokenValidationParameters _tokenValidationParams;

        private BaseResponseMessage response;
        private TokenRequestDTO tokenRequest;

        public UsersController(IOptionsMonitor<JwtConfig> optionsMonitor, TokenValidationParameters tokenValidationParams, IHttpContextAccessor httpContextAccessor)
        {
            _tokenValidationParams = tokenValidationParams;
            _usersAuthentication = new RefreshUserTokenService(optionsMonitor, _tokenValidationParams);
            _adminsAuthentication = new RefreshAdminTokenService(optionsMonitor, _tokenValidationParams);

            response = new BaseResponseMessage();
            tokenRequest = new TokenRequestDTO();
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll([FromHeader] string token, [FromHeader] string refreshToken)
        {
            if (token != null && refreshToken != null)
            {
                tokenRequest.Token = token;
                tokenRequest.RefreshToken = refreshToken;

                JwtResult jwtUserToken = await _usersAuthentication.RefreshUserToken(tokenRequest);
                JwtResult jwtAdminToken = await _adminsAuthentication.RefreshAdminToken(tokenRequest);

                if (jwtUserToken.JwtSuccess == true)
                {
                    response.Code = 201;

                    response.Body = await UsersManagementService.GetAll();

                    HttpContext.Response.Headers.Add("token", jwtUserToken.Token);
                    HttpContext.Response.Headers.Add("refreshToken", jwtUserToken.RefreshToken);

                    response.JwtSuccess = jwtUserToken.JwtSuccess;
                    response.JwtErrors = jwtUserToken.JwtErrors;
                }
                else if (jwtAdminToken.JwtSuccess == true)
                {
                    response.Code = 201;

                    response.Body = await UsersManagementService.GetAll();

                    HttpContext.Response.Headers.Add("token", jwtAdminToken.Token);
                    HttpContext.Response.Headers.Add("refreshToken", jwtAdminToken.RefreshToken);

                    response.JwtSuccess = jwtAdminToken.JwtSuccess;
                    response.JwtErrors = jwtAdminToken.JwtErrors;
                }
                else if (jwtUserToken.JwtSuccess != true)
                {
                    response.Code = 200;

                    response.JwtSuccess = jwtUserToken.JwtSuccess;
                    response.JwtErrors = jwtUserToken.JwtErrors;
                }
                else if (jwtAdminToken.JwtSuccess != true)
                {
                    response.Code = 200;

                    response.JwtSuccess = jwtAdminToken.JwtSuccess;
                    response.JwtErrors = jwtAdminToken.JwtErrors;
                }
            }
            else
            {
                response.Code = 400;
                response.Error = "Missing data - Token or refresh token is null or invalid!";
            }

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(int id, [FromHeader] string token, [FromHeader] string refreshToken)
        {
            if (id == 0 || token == null || refreshToken == null)
            {
                response.Code = 400;
                response.Error = "Missing data - Id and/or token data is incorrect or null";

                return new JsonResult(response);
            }
            else
            {
                tokenRequest.Token = token;
                tokenRequest.RefreshToken = refreshToken;

                JwtResult jwtUserToken = await _usersAuthentication.RefreshUserToken(tokenRequest);
                JwtResult jwtAdminToken = await _adminsAuthentication.RefreshAdminToken(tokenRequest);

                if (jwtUserToken.JwtSuccess == true)
                {
                    response.Code = 201;

                    response.Body = await UsersManagementService.GetById(id);

                    HttpContext.Response.Headers.Add("token", jwtUserToken.Token);
                    HttpContext.Response.Headers.Add("refreshToken", jwtUserToken.RefreshToken);

                    response.JwtSuccess = jwtUserToken.JwtSuccess;
                    response.JwtErrors = jwtUserToken.JwtErrors;
                }
                else if (jwtAdminToken.JwtSuccess == true)
                {
                    response.Code = 201;

                    response.Body = await UsersManagementService.GetById(id);

                    HttpContext.Response.Headers.Add("token", jwtAdminToken.Token);
                    HttpContext.Response.Headers.Add("refreshToken", jwtAdminToken.RefreshToken);

                    response.JwtSuccess = jwtAdminToken.JwtSuccess;
                    response.JwtErrors = jwtAdminToken.JwtErrors;
                }
                else if (jwtUserToken.JwtSuccess != true)
                {
                    response.Code = 200;

                    response.JwtSuccess = jwtUserToken.JwtSuccess;
                    response.JwtErrors = jwtUserToken.JwtErrors;
                }
                else if (jwtAdminToken.JwtSuccess != true)
                {
                    response.Code = 200;

                    response.JwtSuccess = jwtAdminToken.JwtSuccess;
                    response.JwtErrors = jwtAdminToken.JwtErrors;
                }

                return new JsonResult(response);
            }
        }

        [HttpGet]
        [Route("Login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (email == null || password == null)
            {
                response.Code = 400;
                response.Error = "Missing data - Email or password is empty!";

                return new JsonResult(response);
            }
            else
            {
                List<UserDTO> users = await UsersManagementService.GetAll();

                UserDTO? user = users
                    .Where(a => a.Email == email && StringCipher.Decrypt(a.Password, EncriptionVariables.PasswordEncriptionKey) == password).FirstOrDefault();

                if (user == null)
                {
                    response.Code = 200;
                    response.Error = "Missing data - User does not exist.";
                }
                else
                {
                    UserDTO userDTO = user;

                    await _usersAuthentication.ClearAllUserTokens(userDTO);
                    JwtResult jwtToken = await _usersAuthentication.GenerateUserJwtToken(userDTO);

                    HttpContext.Response.Headers.Add("token", jwtToken.Token);
                    HttpContext.Response.Headers.Add("refreshToken", jwtToken.RefreshToken);

                    response.Code = 201;
                    response.Body = userDTO;
                    response.JwtSuccess = jwtToken.JwtSuccess;
                    response.JwtErrors = jwtToken.JwtErrors;
                }
            }

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp(UserDTO signUpDTO)
        {
            if (string.IsNullOrEmpty(signUpDTO.Email) || string.IsNullOrEmpty(signUpDTO.Password) 
                || string.IsNullOrEmpty(signUpDTO.FirstName) || string.IsNullOrEmpty(signUpDTO.LastName)
                || string.IsNullOrEmpty(signUpDTO.Gender))
            {
                response.Code = 400;
                response.Error = "Missing data - User data is incomplete!";

                return new JsonResult(response);
            }
            else if (await UsersManagementService.VerifyEmail(signUpDTO.Email) == true)
            {
                response.Code = 200;
                response.Body = "User with this email already exists!";
            }
            else
            {
                signUpDTO.Password = StringCipher.Encrypt(signUpDTO.Password, EncriptionVariables.PasswordEncriptionKey);
                await UsersManagementService.Save(signUpDTO);

                response.Code = 201;
                response.Body = "User has been signed up!";
            }

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(UserDTO userDTO, [FromHeader] string token, [FromHeader] string refreshToken)
        {
            if (token != null && refreshToken != null && userDTO.Id != 0)
            {
                tokenRequest.Token = token;
                tokenRequest.RefreshToken = refreshToken;

                JwtResult jwtUserToken = await _usersAuthentication.RefreshUserToken(tokenRequest);
                JwtResult jwtAdminToken = await _adminsAuthentication.RefreshAdminToken(tokenRequest);

                if (jwtUserToken.JwtSuccess == true)
                {
                    response.Code = 201;

                    await UsersManagementService.Save(userDTO);
                    response.Body = "User has been updated!";

                    HttpContext.Response.Headers.Add("token", jwtUserToken.Token);
                    HttpContext.Response.Headers.Add("refreshToken", jwtUserToken.RefreshToken);

                    response.JwtSuccess = jwtUserToken.JwtSuccess;
                    response.JwtErrors = jwtUserToken.JwtErrors;
                }
                else if (jwtAdminToken.JwtSuccess == true)
                {
                    response.Code = 201;

                    await UsersManagementService.Save(userDTO);
                    response.Body = "User has been updated!";

                    HttpContext.Response.Headers.Add("token", jwtAdminToken.Token);
                    HttpContext.Response.Headers.Add("refreshToken", jwtAdminToken.RefreshToken);

                    response.JwtSuccess = jwtAdminToken.JwtSuccess;
                    response.JwtErrors = jwtAdminToken.JwtErrors;
                }
                else if (jwtUserToken.JwtSuccess != true)
                {
                    response.Code = 200;

                    response.JwtSuccess = jwtUserToken.JwtSuccess;
                    response.JwtErrors = jwtUserToken.JwtErrors;
                }
                else if (jwtAdminToken.JwtSuccess != true)
                {
                    response.Code = 200;

                    response.JwtSuccess = jwtAdminToken.JwtSuccess;
                    response.JwtErrors = jwtAdminToken.JwtErrors;
                }
            }

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout(int userId)
        {
            UserDTO user = await UsersManagementService.GetById(userId);

            await _usersAuthentication.ClearAllUserTokens(user);

            if (await _usersAuthentication.GetUserTokensCount(user) == 0)
            {
                response.Code = 201;
            }
            else
            {
                response.Code = 200;

                response.Error = "Something happend!";
            }

            return new JsonResult(response);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id, [FromHeader] string token, [FromHeader] string refreshToken)
        {
            if (id == 0 || token == null || refreshToken == null)
            {
                response.Code = 400;
                response.Error = "Missing data - Id and/or token data is incorrect or null";

                return new JsonResult(response);
            }
            else
            {
                tokenRequest.Token = token;
                tokenRequest.RefreshToken = refreshToken;

                JwtResult jwtToken = await _adminsAuthentication.RefreshAdminToken(tokenRequest);

                if (jwtToken.JwtSuccess == true)
                {
                    response.Code = 201;

                    await UsersManagementService.Delete(id);
                    response.Body = "User has been deleted!";

                    HttpContext.Response.Headers.Add("token", jwtToken.Token);
                    HttpContext.Response.Headers.Add("refreshToken", jwtToken.RefreshToken);

                    response.JwtSuccess = jwtToken.JwtSuccess;
                    response.JwtErrors = jwtToken.JwtErrors;
                }
                else
                {
                    response.Code = 200;

                    response.JwtSuccess = jwtToken.JwtSuccess;
                    response.JwtErrors = jwtToken.JwtErrors;
                }

                response.Error = "Something went wrong, User has NOT been deleted!";
            }

            return new JsonResult(response);
        }
    }
}

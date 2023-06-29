using Authorisation.Configuration;
using Authorisation.DTOs;
using Authorisation.Services;
using FitCookieAI_API.Responses;
using FitCookieAI_ApplicationService.DTOs.AdminRelated;
using FitCookieAI_ApplicationService.Implementations.AdminRelated;
using GlobalVariables.Encription;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FitCookieAI_API.Controllers.AdminRelated
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly RefreshAdminTokenService _authentication;
        private readonly TokenValidationParameters _tokenValidationParams;

        private BaseResponseMessage response;
        private TokenRequestDTO tokenRequest;

        public AdminsController(IOptionsMonitor<JwtConfig> optionsMonitor, TokenValidationParameters tokenValidationParams, IHttpContextAccessor httpContextAccessor)
        {
            _tokenValidationParams = tokenValidationParams;
            _authentication = new RefreshAdminTokenService(optionsMonitor, _tokenValidationParams);

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

                JwtResult jwtAdminToken = await _authentication.RefreshAdminToken(tokenRequest);

                if (jwtAdminToken.JwtSuccess == true)
                {
                    response.Code = 201;

                    response.Body = await AdminsManagementService.GetAll();

                    HttpContext.Response.Headers.Add("token", jwtAdminToken.Token);
                    HttpContext.Response.Headers.Add("refreshToken", jwtAdminToken.RefreshToken);

                    response.JwtSuccess = jwtAdminToken.JwtSuccess;
                    response.JwtErrors = jwtAdminToken.JwtErrors;
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

                JwtResult jwtAdminToken = await _authentication.RefreshAdminToken(tokenRequest);

                if (jwtAdminToken.JwtSuccess == true)
                {
                    response.Code = 201;

                    response.Body = await AdminsManagementService.GetById(id);

                    HttpContext.Response.Headers.Add("token", jwtAdminToken.Token);
                    HttpContext.Response.Headers.Add("refreshToken", jwtAdminToken.RefreshToken);

                    response.JwtSuccess = jwtAdminToken.JwtSuccess;
                    response.JwtErrors = jwtAdminToken.JwtErrors;
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
            BaseResponseMessage response = new BaseResponseMessage();

            if (email == null || password == null)
            {
                response.Code = 400;
                response.Error = "Missing data - Email or password is empty!";

                return new JsonResult(response);
            }
            else
            {
                List<AdminDTO> admins = await AdminsManagementService.GetAll();

                AdminDTO? admin = admins
                    .Where(a => a.Email == email && StringCipher.Decrypt(a.Password, EncriptionVariables.PasswordEncriptionKey) == password)
                    .FirstOrDefault();

                if (admin == null)
                {
                    response.Code = 200;
                    response.Error = "Missing data - Admin does not exist.";
                }
                else
                {
                    AdminDTO adminDTO = admin;

                    await _authentication.ClearAllAdminTokens(adminDTO);
                    JwtResult jwtAdminToken = await _authentication.GenerateAdminJwtToken(adminDTO);

                    HttpContext.Response.Headers.Add("token", jwtAdminToken.Token);
                    HttpContext.Response.Headers.Add("refreshToken", jwtAdminToken.RefreshToken);

                    response.Code = 201;
                    response.Body = adminDTO;
                    response.JwtSuccess = jwtAdminToken.JwtSuccess;
                    response.JwtErrors = jwtAdminToken.JwtErrors;
                }
            }

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp(AdminDTO adminDTO)
        {
            BaseResponseMessage response = new BaseResponseMessage();

            if (adminDTO.FirstName == null || adminDTO.LastName == null
                || adminDTO.Email == null || adminDTO.Password == null ||
                AdminsManagementService.VerifyEmail(adminDTO.Email).Equals(true))
            {
                response.Code = 400;
                response.Error = "Missing data - Admin data is incomplete or admin email Already Exists!";

                return new JsonResult(response);
            }
            else
            {
                adminDTO.Password = StringCipher.Encrypt(adminDTO.Password, EncriptionVariables.PasswordEncriptionKey);

				await AdminsManagementService.Save(adminDTO);
                response.Code = 201;
                response.Body = "Admin has been signed up!";
            }

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(AdminDTO adminDTO, [FromHeader] string token, [FromHeader] string refreshToken)
        {
            BaseResponseMessage response = new BaseResponseMessage();

            if (token == null || refreshToken == null)
            {
                response.Code = 400;
                response.Error = "Missing data - Id and/or token data is incorrect or null";

                return new JsonResult(response);
            }
            else
            {
                if (adminDTO.FirstName == null || adminDTO.LastName == null
                || adminDTO.Email == null || AdminsManagementService.VerifyEmail(adminDTO.Email).Equals(true))
                {
                    response.Code = 400;
                    response.Error = "Missing data - Admin data is incomplete or admin email Already Exists!";

                    return new JsonResult(response);
                }
                else
                {
                    tokenRequest.Token = token;
                    tokenRequest.RefreshToken = refreshToken;

                    JwtResult jwtAdminToken = await _authentication.RefreshAdminToken(tokenRequest);

                    if (jwtAdminToken.JwtSuccess == true)
                    {
                        adminDTO.Password = StringCipher.Encrypt(adminDTO.Password, EncriptionVariables.PasswordEncriptionKey);

                        await AdminsManagementService.Save(adminDTO);
                        response.Code = 201;
                        response.Body = "Admin has been saved!";

                        HttpContext.Response.Headers.Add("token", jwtAdminToken.Token);
                        HttpContext.Response.Headers.Add("refreshToken", jwtAdminToken.RefreshToken);
                    }
                    else
                    {
                        response.Code = 201;
                    }

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
            AdminDTO admin = await AdminsManagementService.GetById(userId);

            await _authentication.ClearAllAdminTokens(admin);

            if (await _authentication.GetAdminTokensCount(admin) == 0)
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

                JwtResult jwtAdminToken = await _authentication.RefreshAdminToken(tokenRequest);

                if (jwtAdminToken.JwtSuccess == true)
                {
                    response.Code = 201;

                    await AdminsManagementService.Delete(id);
                    response.Body = "Admin has been deleted!";

                    HttpContext.Response.Headers.Add("token", jwtAdminToken.Token);
                    HttpContext.Response.Headers.Add("refreshToken", jwtAdminToken.RefreshToken);
                }
                else
                {
                    response.Code = 200;
                }

                response.JwtSuccess = jwtAdminToken.JwtSuccess;
                response.JwtErrors = jwtAdminToken.JwtErrors;
            }

            return new JsonResult(response);
        }
    }
}

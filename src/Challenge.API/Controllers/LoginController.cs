using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Challenge.API.Controllers.Base;
using Challenge.Application.Business.Users.Entities;
using Challenge.Application.Business.Users.Queries;
using Challenge.Application.Business.Users.Response;
using Challenge.Common;
using Challenge.Core.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Challenge.API.Controllers
{
    public class LoginController : BaseController
    {
        private readonly Dispatcher _dispatcher;

        public LoginController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpQuery query)
        {
            var response = new BaseResponse<bool>();
            query.IpAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var user = await _dispatcher.Dispatch(query);

            response.SetMessage("Başarıyla kayıt olundu.");

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SignInWithEmail([FromBody] SignInWithEmailQuery query)
        {
            var response = new BaseResponse<SignInResponse>();
            query.IpAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var user = await _dispatcher.Dispatch(query);
            response.Data = new SignInResponse { Token = WriteToken(user) };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SignInWithApple([FromBody] SignInWithAppleQuery query)
        {
            var response = new BaseResponse<SignInResponse>();
            query.IpAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var user = await _dispatcher.Dispatch(query);
            response.Data = new SignInResponse { Token = WriteToken(user) };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SignInWithGoogle([FromBody] SignInWithGoogleQuery query)
        {
            var response = new BaseResponse<SignInResponse>();
            query.IpAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var user = await _dispatcher.Dispatch(query);
            response.Data = new SignInResponse { Token = WriteToken(user) };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SignInWithFacebook([FromBody] SignInWithFacebookQuery query)
        {
            var response = new BaseResponse<SignInResponse>();
            query.IpAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var user = await _dispatcher.Dispatch(query);
            response.Data = new SignInResponse { Token = WriteToken(user) };

            return Ok(response);
        }

        [NonAction]
        private string WriteToken(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("userId", user.Id));
            claims.Add(new Claim(ClaimTypes.Role, user.Role.ToString()));

            var token = new JwtSecurityToken(
                  issuer: "i",
                  audience: "a",
                  claims: claims,
                  expires: DateTime.UtcNow.AddDays(30),
                  notBefore: DateTime.UtcNow,
                  signingCredentials:
                      new SigningCredentials(
                          new SymmetricSecurityKey(
                              Encoding.UTF8.GetBytes("all4baby2021!!1!1!!1pwd")), SecurityAlgorithms.HmacSha256)
              );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
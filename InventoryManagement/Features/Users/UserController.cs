using InventoryManagement.Classes;
using InventoryManagement.Enums;
using InventoryManagement.Exceptions;
using InventoryManagement.Features.Logins.Models;
using InventoryManagement.Features.Logins.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Logins
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService loginService)
        {
            _userService = loginService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<OkResult>> RegisterUser(RegisterResource registerUser)
        {
            var register = await _userService.RegisterUser(registerUser);
            return register switch
            {
                Message.UserCreatedVerifyMail => Ok(register),
                Message.IdNotFound => NotFound(new IdNotFoundException()),
                Message.UserAlreadyExists => BadRequest(new UserAlreadyExistsException()),
                Message.UsernameAlreadyExists => BadRequest(new UserAlreadyExistsException(registerUser.UserName)),
                _ => null,
            };
        }

        [AllowAnonymous]
        [HttpPut("updateLogin")]
        public async Task<ActionResult<OkResult>> UpdateLogin(string employeeId, [FromForm] UpdateUser update)
        {
            var login = await _userService.Update(employeeId, update);
            if (login == null) return NotFound(new IdNotFoundException());

            return Ok(Message.LoginCredentialsUpdated);
        }

/*        [Authorize(Roles = "User")]*/
        [HttpGet]
        public async Task<ActionResult<OkResult>> GetUsers([FromQuery] GetUsers users)
        {
            var getUsers = await _userService.GetUsers(users);

            return Ok(getUsers);
        }

        /*[Authorize(Roles = "User")]*/
        [HttpPut("deleteusers")]
        public async Task<ActionResult<OkResult>> DeleteUsers(string employeeId)
        {
            var deleteUser = await _userService.DeleteUser(employeeId);
            if (deleteUser == null) return NotFound(new IdNotFoundException());

            return Ok(deleteUser);
        }

        [AllowAnonymous]
        [HttpPost("forgotpassword")]
        public async Task<ActionResult<OkResult>> ForgotPassword(string username)
        {
            var forgot = await _userService.ForgotPassword(username);
            return Ok(forgot);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public ActionResult<OkResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = _userService.RefreshToken(refreshToken, IpAddress());

            if (response == null) return Unauthorized(new { messsage = "Invalid token" });
            
            SetTokenCookie(response.Result.RefreshToken);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("revoke-token")]
        public ActionResult<OkResult> RevokeToken([FromBody] RevokeTokenRequest model)
        {
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token)) return BadRequest(new { message = "Token is required" });
            
            var response = _userService.RevokeToken(token, IpAddress());

            if (!response) return NotFound(new { message = "Token not found" });
            
            return Ok(new { message = "Token revoked" });
        }

        [AllowAnonymous]
        [HttpPost("confirmmail")]
        public async Task<ActionResult<OkResult>> ConfirmMail(string username)
        {
            var confirm = await _userService.ConfirmMail(username);
            return Ok(confirm);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<OkResult>> Login(Login loginCredentials)
        {
            var login = await _userService.Login(loginCredentials,IpAddress());
            switch(login)
            {
                case Message.InvalidUserNameOrPassword:
                    return Unauthorized(new InvalidUsernamePasswordException());
                case UserLogin userLogin:
                    SetTokenCookie(login.RefreshToken);
                    return Ok(userLogin);
                default:
                    return Unauthorized(new EmailNotVerifiedException());
            }
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public ActionResult<OkResult> Logout()
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddSeconds(1),
            };
            Response.Cookies.Delete("Jwt-Token", cookieOptions);
            return NoContent();
        }

        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            }
            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}

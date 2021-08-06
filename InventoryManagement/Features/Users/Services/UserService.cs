using InventoryManagement.Classes;
using InventoryManagement.Context;
using InventoryManagement.Features.Logins.Models;
using InventoryManagement.Helpers;
using InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Logins.Services
{
    internal class UserService : IUserService
    {
        private readonly InventoryDbContext _inventoryDbContext;
        private readonly PasswordEncrypt encrypt = new PasswordEncrypt();
        private readonly Authentication auth = new Authentication();
        private readonly Functions _functions;
        public UserService(InventoryDbContext inventoryDbContext, Functions functions)
        {
            _inventoryDbContext = inventoryDbContext;
            _functions = functions;
        }

        public async Task<string> ConfirmMail(string username)
        {
            try
            {
                var check = _inventoryDbContext.Users.Any(log => log.UserName == username);
                if (string.IsNullOrEmpty(username)) return "Invalid username";
                if (check)
                {
                    var update = _inventoryDbContext.Users.Where(log => log.UserName == username).FirstOrDefault();
                    update.IsMailConfirmed = true;
                    _inventoryDbContext.SaveChanges();

                    return await Task.FromResult(Message.Success);
                }

                return await Task.FromResult(Message.InvalidUser);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<IEnumerable<UserResource>> GetUsers(GetUsers getUsers)
        {
            IEnumerable<UserResource> users;
            if (_functions.ParameterNullChecker(getUsers))
            {
                users = _inventoryDbContext.Users.Where(user => user.IsDeleted == 0).Select(MapToUserResource);
            }
            else
            {
                users = _inventoryDbContext.Users.Where(user => ((getUsers.EmployeeId == null || user.EmployeeId == getUsers.EmployeeId) 
                                                             && (getUsers.UserName == null || user.UserName == getUsers.UserName)
                                                             && (getUsers.Usertypes == null || user.UserType == $"{getUsers.Usertypes}") 
                                                             && (getUsers.IsConfirmed == null || user.IsMailConfirmed == getUsers.IsConfirmed)
                                                             && user.IsDeleted == 0)).Select(MapToUserResource);
            }
            return await Task.FromResult(users);
        }

        public async Task<dynamic> Login(Login loginCredentials, string ipAddress)
        {
            var ecryptPassword = encrypt.Encrypt(loginCredentials.Password);
            var finduser = _inventoryDbContext.Users.Where(user => user.UserName == loginCredentials.Username && user.Password == ecryptPassword).FirstOrDefault();

            if (finduser == null) return await Task.FromResult(Message.InvalidUserNameOrPassword);

            if (finduser.IsMailConfirmed)
            {
                var employee = _inventoryDbContext.Employee.Where(emp => emp.EmployeeId == finduser.EmployeeId).FirstOrDefault();
                var refreshToken = auth.GenerateRefreshToken(ipAddress);
                var jwtToken = auth.GenerateJwtToken(finduser);
                finduser.RefreshTokens.Add(refreshToken);
                _inventoryDbContext.SaveChanges();
                return await Task.FromResult(new UserLogin(finduser.Id, _functions.MaptoEmployeeDetails(employee), jwtToken, refreshToken.Token));
            }
            return await Task.FromResult(Message.VerifyEmail);

        }

        public async Task<UserLogin> RefreshToken(string token, string ipAddress)
        {
            var user = _inventoryDbContext.Users.SingleOrDefault(users => users.RefreshTokens.Any(t => t.Token == token));
            if (user == null) return null;

            var employee = _inventoryDbContext.Employee.SingleOrDefault(emp => emp.EmployeeId == user.EmployeeId);
            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive) return null;

            var newRefreshToken = auth.GenerateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);
            _inventoryDbContext.Update(user);
            _inventoryDbContext.SaveChanges();
            var jwtToken = auth.GenerateJwtToken(user);

            return await Task.FromResult(new UserLogin(user.Id, _functions.MaptoEmployeeDetails(employee), jwtToken, newRefreshToken.Token));
        }

        public bool RevokeToken(string token, string ipAddress)
        {
            var user = _inventoryDbContext.Users.SingleOrDefault(users => users.RefreshTokens.Any(tokens => tokens.Token == token));
            if (user == null) return false;
            var refreshToken = user.RefreshTokens.Single(tokens => tokens.Token == token);
            if (!refreshToken.IsActive) return false;
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            _inventoryDbContext.Update(user);
            _inventoryDbContext.SaveChanges();
            return true;
        }

        public async Task<string> RegisterUser(RegisterResource registerUser)
        {
            var findId = _inventoryDbContext.Employee.Where(emp => emp.EmployeeId == registerUser.EmployeeId).FirstOrDefault();
            if (findId == null) return await Task.FromResult(Message.IdNotFound);

            string url = string.Format("{0}api/User/confirmmail?username={1}", Global.DomainName, registerUser.UserName);
            var checkUser = _inventoryDbContext.Users.Any(user => user.EmployeeId == $"{findId.Id}");
            var checkUserName = _inventoryDbContext.Users.Any(login => login.UserName == registerUser.UserName);
            var body = GetMailBody(url: url, username: registerUser.UserName, buttonName: "Confirmation Link",
                                  text: "Please click below button for confirm your email.", type: "Email Confirmation");

            if (checkUserName) return await Task.FromResult(Message.UsernameAlreadyExists);

            if (checkUser) return await Task.FromResult(Message.UserAlreadyExists);

                var register = new Users
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = $"{findId.Id}",
                    UserName = registerUser.UserName,
                    Password = encrypt.Encrypt(registerUser.Password),
                    UserType = findId.UserType,
                    IsMailConfirmed = false,
                    IsDeleted = 0
                };
                SendMessage(findId, "Email Confirmation", body);
                _inventoryDbContext.Users.Add(register);
                _inventoryDbContext.SaveChanges();
                return await Task.FromResult(Message.UserCreatedVerifyMail);
        }

        public async Task<string> ForgotPassword(string username)
        {
            var finduser = _inventoryDbContext.Users.Where(user => user.UserName == username).FirstOrDefault();
            var findId = _inventoryDbContext.Employee.Where(emp => $"{emp.Id}" == finduser.EmployeeId).SingleOrDefault();
            string url = string.Format("{0}api/User/confirmmail?username={1}", Global.DomainName, username);
            var body = GetMailBody(url: url,
                                   username: username,
                                   buttonName: "Password Reset Link",
                                   text: "You've requested to reset your password. Click the link below to reset your password.Ignore if you didn't authorize this.",
                                   type: "Password Reset");
            SendMessage(findId, "Password Reset", body);
            return await Task.FromResult(Message.ForgotPassword);
        }

        public async Task<string> Update(string employeeId, [FromForm] UpdateUser update)
        {
            var findLogin = _inventoryDbContext.Users.Where(log => log.EmployeeId == employeeId);
            if (findLogin == null) return null;

            _functions.UpdateDetails(employeeId, findLogin, update);
            _inventoryDbContext.SaveChanges();
            return await Task.FromResult(Message.LoginCredentialsUpdated);

        }

        public async Task<string> DeleteUser(string employeeId)
        {
            var user = _inventoryDbContext.Users.Where(user => user.EmployeeId == employeeId && user.IsDeleted == 0).FirstOrDefault();
            if (user == null) return null;

            user.IsDeleted = 1;
            var delete = string.Format("Deleted user {0}", user.UserName);
            return await Task.FromResult(delete);
        }

        private UserResource MapToUserResource(Users users)
        {
            return new UserResource
            {
                EmployeeId = users.EmployeeId,
                UserName = users.UserName,
                UserType = users.UserType,
                IsMailConfirmed = users.IsMailConfirmed
            };
        }

        private void SendMessage(Employee employeeDetails, string subject, string messageBody)
        {
            MailClass mail = new MailClass();
            var fromAddress = new MailAddress(mail.FromMailId, "Yoonet");
            var toAddress = new MailAddress(employeeDetails.Email, employeeDetails.FirstName);
            string fromPassword = mail.FromMailPassword;
            string body = messageBody;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            smtp.Send(message);
        }

        private string GetMailBody(string url, string username, string buttonName, string text, string type)
        {

            return string.Format(@" <body style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; -webkit-font-smoothing: antialiased; -webkit-text-size-adjust: none; width: 100% !important; height: 100%; line-height: 1.6em; background-color: #f6f6f6; margin: 0;"" bgcolor=""#f6f6f6"">  
                                    <table class=""body-wrap"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; width: 100%; background-color: #f6f6f6; margin: 0;"" bgcolor=""#f6f6f6"">  
                                    <tr style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;"">
                                    <td style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0;"" valign=""top""></td>  
                                    <td class=""container"" width=""600"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; display: block !important; max-width: 600px !important; clear: both !important; margin: 0 auto;"" valign=""top"">  
                                    <div class=""content"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; max-width: 600px; display: block; margin: 0 auto; padding: 20px;"">  
                                    <table class=""main"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; border-radius: 3px; background-color: #fff; margin: 0; border: 1px solid #e9e9e9;"" bgcolor=""#fff"">  
                                    <tr style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;"">
                                    <td class=""alert alert-warning"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 16px; vertical-align: top; color: #fff; font-weight: 500; text-align: center; border-radius: 3px 3px 0 0; background-color: #3c8dbc; margin: 0; padding: 20px;"" align=""center"" bgcolor=""#FF9F00"" valign=""top"">  
                                    {4}
                                    </td>  
                                    </tr>  
                                    <tr style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;"">
                                    <td class=""content-wrap"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0; padding: 20px;"" valign=""top"">  
                                    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;"">  
                                    <tr style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;"">
                                    <td class=""content-block"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0; padding: 0 0 20px;"" valign=""top"">  
                                    Hello, <strong style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;"">{1}</strong>  
                                    </td>  
                                    </tr>  
                                    <tr style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;"">
                                    <td class=""content-block"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0; padding: 0 0 20px;"" valign=""top"">  
                                    {3}
                                    </td>  
                                    </tr>  
                                    <tr style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;"">
                                    <td class=""content-block"" style="""">  
                                    <form method='post' action='{0}' style='display: inline;'>
                                    <button class=""btn-primary"" style=""background-color: #3c8dbc; border: none;color: white; padding: 10px 41px;text-align: center;text-decoration: none;display: inline-block; font-size: 16px;margin: 4px 2px;cursor: pointer;"">{2}</button>
                                     </form>
                                    </td>  
                                    </tr>  
                                    <tr style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;"">
                                      <td class=""content-block"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0; padding: 0 0 20px;"" valign=""top"">  
                                    <br/><b>Thanks & Regards,</b><br/>  
                                    Yoonet PH<br/>  
                                    </td>  
                                    </tr>  
                                    </table>  
                                    </td>  
                                    </tr>  
                                    </table><div class=""footer"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; width: 100%; clear: both; color: #999; margin: 0; padding: 20px;"">  
                                    <table width=""100%"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;"">  
                                    <tr style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;"">
                                    <td class=""aligncenter content-block"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 12px; vertical-align: top; color: #999; text-align: center; margin: 0; padding: 0 0 20px;"" align=""center"" valign=""top""></td>  
                                    </tr>  
                                    </table>  
                                    </div>  
                                    </div>  
                                    </td>  
                                    <td style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0;"" valign=""top""></td>  
                                    </tr>  
                                    </table>  
                                    </body> ", url, username, buttonName, text, type);
        }
    }
}

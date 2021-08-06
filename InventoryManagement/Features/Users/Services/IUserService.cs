using InventoryManagement.Enums;
using InventoryManagement.Features.Logins.Models;
using InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Features.Logins.Services
{
    public interface IUserService
    {
        Task<string> RegisterUser(RegisterResource registerUser);
        Task<IEnumerable<UserResource>> GetUsers(GetUsers getUsers);
        Task<dynamic> Login(Login loginCredentials,string ipAddress);
        Task<string> ConfirmMail(string username);
        Task<string> ForgotPassword(string username);
        Task<string> Update(string employeeId, [FromForm] UpdateUser update);
        Task<string> DeleteUser(string employeeId);
        Task<UserLogin> RefreshToken(string token, string ipAddress);
        bool RevokeToken(string token, string ipAddress);
    }
}

using InventoryManagement.Enums;
using InventoryManagement.Features.Employees.Models;
using InventoryManagement.Models;
using System;
using System.Text.Json.Serialization;

namespace InventoryManagement.Features.Logins.Models
{
    public class RegisterResource
    {
        public string EmployeeId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class GetUsers
    {
        public string EmployeeId { get; set; }
        public string UserName { get; set; }
        public UserTypes? Usertypes { get; set; }
        public bool? IsConfirmed { get; set; }
    }

    public class UserLogin
    {
        public Guid Id { get; set; }
        public EmployeeDetails<string> Employee { get; set; }
        public string Token { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }

        public UserLogin (Guid id,EmployeeDetails<string> employee, string jwtToken, string refreshToken)
        {
            Id = id;
            Employee = employee;
            Token = jwtToken;
            RefreshToken = refreshToken;
        }
    }
    public class UpdateUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public bool IsMailConfirmed { get; set; }
    }
    public class UserResource
    {
        public string EmployeeId { get; set; }
        public string UserName { get; set; }
        public string UserType { get; set; }
        public bool IsMailConfirmed { get; set; }
    }
    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class RevokeTokenRequest
    {
        public string Token { get; set; }
    }
}

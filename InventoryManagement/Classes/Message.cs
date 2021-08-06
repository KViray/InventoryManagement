using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Classes
{
    public static class Message
    {
        public static string Success = "Email successfully verified";
        public static string ErrorFound = "Error Found";
        public static string LoginCredentialsUpdated = "Login credentials successfully updated";
        public const string VerifyEmail = "User already created please verify your email";
        public static string InvalidUser = "Invalid user. Please create account";
        public static string MailSent = "Mail Sent";
        public const string UserCreatedVerifyMail = "User created, check mail. Click link and verify";
        public const string IdNotFound = "Id Not Found";
        public const string InvalidUserNameOrPassword = "Invalid Username or Password";
        public const string UserAlreadyExists = "User already exists";
        public const string UsernameAlreadyExists = "Username already exists";
        public const string ForgotPassword = "The link was sent to your email to reset your password";
    }
}

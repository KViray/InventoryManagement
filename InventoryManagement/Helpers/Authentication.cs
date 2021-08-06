using InventoryManagement.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InventoryManagement.Helpers
{
    public class Authentication
    {
        public string GenerateJwtToken(Users employee)
        {
            string securityKey = "n=G!&*iAuehpV8UTuC/li_g(/jS;gA3q%%bDZ9!I>RZHjyZtRQVTeS>QL*C#Zfy.$yoonet.com.au";
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var signingCredential = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var token = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, $"{employee.EmployeeId}"),
                    new Claim(ClaimTypes.Role, $"{employee.UserType}")
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = signingCredential,
                Issuer = "Yoonet",
                IssuedAt = DateTime.UtcNow
                
            };
            var stoken = new JwtSecurityTokenHandler().CreateToken(token);
            return new JwtSecurityTokenHandler().WriteToken(stoken);
        }
        public RefreshToken GenerateRefreshToken(string ipAddress)
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }
    }
}

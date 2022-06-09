using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ProcessSystem.Token
{
    public class TokenImpl : IToken
    {
        public const string Issuer = "Test"; // издатель токена
        public const string Audience = "Test"; // потребитель токена
        public const string Key = "mysupersecret_secretkey!123";   // ключ для шифрации

        public string GenerateToken()
        {
            var claims = new[] 
            {
              new Claim(JwtRegisteredClaimNames.Sub, "user"),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(Issuer,
              Audience,
              claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

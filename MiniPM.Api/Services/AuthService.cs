using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniPM.Api.Data;
using MiniPM.Api.DTOs.Auth;
using MiniPM.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MiniPM.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _cfg;

        public AuthService(AppDbContext db, IConfiguration cfg)
        {
            _db = db;
            _cfg = cfg;
        }

        public async Task<User?> Register(RegisterDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
                return null;

            using var hmac = new HMACSHA512();
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(dto.Password)),
                PasswordSalt = hmac.Key
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<string?> Login(LoginDto dto)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null) return null;

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computed = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(dto.Password));
            if (!computed.SequenceEqual(user.PasswordHash)) return null;

            // generate JWT
            var key = Encoding.UTF8.GetBytes(_cfg["Jwt:Key"] ?? throw new InvalidOperationException());
            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };
            var token = new JwtSecurityToken(
                issuer: _cfg["Jwt:Issuer"],
                audience: _cfg["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_cfg["Jwt:ExpireMinutes"] ?? "1440")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

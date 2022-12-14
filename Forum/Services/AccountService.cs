using AutoMapper;
using Forum.Entities;
using Forum.Exceptions;
using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Forum.Services
{


    public class AccountService : IAccountService
    {
        private readonly ForumDbContext dbContext;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly AuthenticationSettings authenticationSettings;
        private readonly IMapper mapper;

        public AccountService(ForumDbContext dbContext, IPasswordHasher<User> passwordHasher
            , AuthenticationSettings authenticationSettings, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.passwordHasher = passwordHasher;
            this.authenticationSettings = authenticationSettings;
            this.mapper = mapper;
        }
        public async Task <string> GenerateJWT(LoginDto dto)
        {
            var user = await dbContext.Users
                .Include(r => r.Role)
                .FirstOrDefaultAsync(u => u.Nick == dto.Nick);

            if (user == null)
            {
                throw new BadRequestException("Invalid username or password");
            }
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("DateOfBirth", user.DateOfBirth.ToString("yyyy-MM-dd")),
                new Claim("Email", user.Email),

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(authenticationSettings.JwtIssuer,
                authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);

        }

        public async Task RegisterUser(CreateUserDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                Nick = dto.Nick,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                RoleId = dto.RoleId,
            };
            var hasherPassword = passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hasherPassword;
            await dbContext.AddAsync(newUser);
            await dbContext.SaveChangesAsync();

        }

        public async Task<UserDto> GetById(int accountId)
        {
            var user =  await dbContext.Users
                .FirstOrDefaultAsync(x => x.Id == accountId);
            if (user is null)
            {
                throw new NotFoundException("User not founded");
            }
            var userDto = mapper.Map<UserDto>(user);
            return userDto;
        }
    }
}

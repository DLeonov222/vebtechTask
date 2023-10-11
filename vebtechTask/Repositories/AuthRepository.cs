using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using vebtechTask.Repositories.Interfaces;
using vebtechTask.Data;
using vebtechTask.Models;
using vebtechTask.Models.Configurations;
using vebtechTask.Models.DTO;
using vebtechTask.CustomException;

namespace vebtechTask;

public class AuthRepository : IAuthRepository
{
    private readonly ApplicationDbContext _context;
    private readonly JwtConfig _jwtConfig;
    private readonly Mapper _mapper;

    public AuthRepository(ApplicationDbContext context, JwtConfig jwtConfig, Mapper mapper)
    {
        _context = context;
        _jwtConfig = jwtConfig;
        _mapper = mapper;
    }

    public async Task<Admin> SignIn(AdminDto adminDto)
    {
        var user = await _context.Admins.FirstOrDefaultAsync(user => user.Email == adminDto.Email);

        return user == null || !BCrypt.Net.BCrypt.Verify(adminDto.Password, user.Password)
            ? throw new HttpResponseException(HttpStatusCode.NotFound, "User not found")
            : user;
    }

    public async Task<Admin> SignUp(AdminDto adminDto)
    {
        var admin = _mapper.Map<Admin>(adminDto);

        await _context.Admins.AddAsync(admin);
        await _context.SaveChangesAsync();
        return admin;
    }

    public async Task<bool> IsExistEmail(string email)
    {
        return await _context.Admins.FirstAsync(admin => admin.Email == email) != null;
    }

    public async Task<string> GenerateJwt(AdminDto adminDto)
    {
        var admin = await SignIn(adminDto);
        if (admin == null)
        {
            return null;
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, admin.Email),
        };

        ClaimsIdentity claimsIdentity =
        new(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

        var now = DateTime.UtcNow;
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfig.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken
            (
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                notBefore: now,
                claims: claimsIdentity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(_jwtConfig.LifeTime)),
                signingCredentials: credentials
            );

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }
}
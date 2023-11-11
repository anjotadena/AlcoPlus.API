using AlcoPlus.API.Contracts;
using AlcoPlus.API.Data;
using AlcoPlus.API.Models.Users;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AlcoPlus.API.Repository;

public class AuthManager : IAuthManager
{
    private readonly IMapper _mapper;
    private readonly UserManager<ApiUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
    {
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
    {
        var user = _mapper.Map<ApiUser>(userDto);

        user.UserName = userDto.Email;

        var result = await _userManager.CreateAsync(user, userDto.Password);

        // Add user roles as regular user
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
        }

        return result.Errors;
    }

    public async Task<AuthResponseDto> Login(LoginUserDto loginUserDto)
    {
     
        var user = await _userManager.FindByEmailAsync(loginUserDto.Email);
        var isValidUser = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);

        if (user is null || !isValidUser)
        {
            return null;
        }

        var token = await GenerateJwtToken(user);

        return new AuthResponseDto
        {
            Token = token,
            UserId = user.Id
        };
    }

    private async Task<string> GenerateJwtToken(ApiUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
        var userClaims = await _userManager.GetClaimsAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id),
        }
        .Union(userClaims)
        .Union(roleClaims);

        var token = new JwtSecurityToken(
                            issuer: _configuration["JwtSettings:Issuer"],
                            audience: _configuration["JwtSettings:Audience"],
                            claims: claims,
                            expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                            signingCredentials: credentials
                        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

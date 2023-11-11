using AlcoPlus.API.Contracts;
using AlcoPlus.API.Data;
using AlcoPlus.API.Models.Users;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
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
    private readonly IHttpContextAccessor _httpContextAccessor;
    private ApiUser _user;
    private const string _loginProvider = "AlcoPlusApi";
    private const string _refreshTokenKey = "RefreshToken";

    public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
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
        _user = await _userManager.FindByEmailAsync(loginUserDto.Email);
        var isValidUser = await _userManager.CheckPasswordAsync(_user, loginUserDto.Password);

        if (_user is null || !isValidUser)
        {
            return null;
        }

        var token = await GenerateJwtToken();

        return new AuthResponseDto
        {
            Token = token,
            UserId = _user.Id,
            RefreshToken = await CreateRefreshToken(),
        };
    }

    private async Task<string> GenerateJwtToken()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var roles = await _userManager.GetRolesAsync(_user);
        var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
        var userClaims = await _userManager.GetClaimsAsync(_user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, _user.Email),
            new Claim("uid", _user.Id),
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

    public async Task<string> CreateRefreshToken()
    {
        await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshTokenKey);

        var token = await _userManager.GenerateUserTokenAsync(_user, _loginProvider, _refreshTokenKey);

        await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshTokenKey, token);

        return token;
    }

    public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
        var username = tokenContent.Claims.ToList().FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)?.Value;

        _user = await _userManager.FindByNameAsync(username);

        if (_user is null || _user.Id != request.UserId)
        {
            return null;
        }

        var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(_user, _loginProvider, _refreshTokenKey, request.RefreshToken);

        if (isValidRefreshToken)
        {
            var token = await GenerateJwtToken();

            return new AuthResponseDto
            {
                Token = token,
                UserId = _user.Id,
                RefreshToken = await CreateRefreshToken()
            };
        }

        await _userManager.UpdateSecurityStampAsync(_user);

        return null;
    }

    public async Task<bool> Logout()
    {
        var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token is null)
        {
            return false;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        if (jwtToken is null)
        {
            return false;
        }

        var email = jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value;

        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return false;
        }

        await _userManager.UpdateSecurityStampAsync(user);

        return true;
    }
}

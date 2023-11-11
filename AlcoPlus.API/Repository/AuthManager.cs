using AlcoPlus.API.Contracts;
using AlcoPlus.API.Data;
using AlcoPlus.API.Models.Users;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AlcoPlus.API.Repository;

public class AuthManager : IAuthManager
{
    private readonly IMapper _mapper;
    private readonly UserManager<ApiUser> _userManager;

    public AuthManager(IMapper mapper, UserManager<ApiUser> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
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

    public async Task<bool> Login(LoginUserDto loginUserDto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginUserDto.Email);

            if (user is null)
            {
                return default;
            }

            return await _userManager.CheckPasswordAsync(user, loginUserDto.Password);
        }
        catch (Exception)
        {
        }

        return false;
    }
}

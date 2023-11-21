using AlcoPlus.Core.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace AlcoPlus.Core.Contracts;

public interface IAuthManager
{
    Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto);
    
    Task<AuthResponseDto> Login(LoginUserDto userDto);

    Task<string> CreateRefreshToken();

    Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto authResponseDto);

    Task<bool> Logout();
}

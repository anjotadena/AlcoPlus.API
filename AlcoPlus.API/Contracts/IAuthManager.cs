using AlcoPlus.API.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace AlcoPlus.API.Contracts;

public interface IAuthManager
{
    Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto);
    Task<bool> Login(LoginUserDto userDto);
}

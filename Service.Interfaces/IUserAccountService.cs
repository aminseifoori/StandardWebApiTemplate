using Microsoft.AspNetCore.Identity;
using Shared.Dtos.Users;


namespace Service.Interfaces
{
    public interface IUserAccountService
    {
        Task<IdentityResult> RegisterUser(CreateUserDto createUserDto);
        Task<bool> ValidateUser(LoginDto userForAuth);
        //Task<string> CreateToken(); Some changes will apply to implement refresh token
        Task<TokenDto> CreateToken(bool populateExp);
        Task<TokenDto> RefreshToken(TokenDto tokenDto);
    }
}

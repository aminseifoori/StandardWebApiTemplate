using Microsoft.AspNetCore.Identity;
using Shared.Dtos.Users;


namespace Service.Interfaces
{
    public interface IUserAccountService
    {
        Task<IdentityResult> RegisterUser(CreateUserDto createUserDto);
    }
}

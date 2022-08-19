using Forum.Models;

namespace Forum;

public interface IAccountService
{
    Task Delete(int accountId);
    Task<string> GenerateJWT(LoginDto dto);
    Task<UserDto> GetById(int accountId);
    Task RegisterUser(CreateUserDto dto);
}

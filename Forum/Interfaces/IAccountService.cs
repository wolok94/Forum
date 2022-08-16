using Forum.Models;

namespace Forum;

public interface IAccountService
{
    Task<string> GenerateJWT(LoginDto dto);
    Task<UserDto> GetById(int accountId);
    Task RegisterUser(CreateUserDto dto);
}

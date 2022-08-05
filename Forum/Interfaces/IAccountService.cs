using Forum.Models;

namespace Forum;

public interface IAccountService
{
    Task<string> GenerateJWT(LoginDto dto);
    Task<IEnumerable<UserDto>> GetAll();
    Task<UserDto> GetById(int id);
    Task RegisterUser(CreateUserDto dto);
}

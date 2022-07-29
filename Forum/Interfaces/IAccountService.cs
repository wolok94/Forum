using Forum.Models;

namespace Forum;

public interface IAccountService
{
    Task<string> GenerateJWT(LoginDto dto);
    Task<IEnumerable<GetAllUsersDto>> GetAll();
    Task RegisterUser(CreateUserDto dto);
}

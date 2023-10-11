using vebtechTask.Models;
using vebtechTask.Models.DTO;

namespace vebtechTask.Repositories.Interfaces;

public interface IUserRepository
{
    IEnumerable<User> Get(PaginationParameters paginationParameters,
        SortParameters sortParameters, FilterParameters filterParameters);
    Task<User> Create(UserDto userDto);
    Task<User> GetUser(int id);
    Task Delete(int id);
    Task<User> Update(int id, UserDto userDto);
    Task<bool> IsEmailExist(string email);
    Task<Role> CreateRole(int userId, string name);
}
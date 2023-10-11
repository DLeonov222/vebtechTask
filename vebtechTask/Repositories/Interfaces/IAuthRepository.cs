using vebtechTask.Models;
using vebtechTask.Models.DTO;

namespace vebtechTask.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<Admin> SignIn(AdminDto adminDto);
    Task<Admin> SignUp(AdminDto adminDto);
    Task<bool> IsExistEmail(string email);
    Task<string> GenerateJwt(AdminDto adminDto);
}
using System.Threading.Tasks;
using Application.Models;

namespace Application.Services;

public interface IAuthService
{
    Task<LoginResult> Login(LoginModel loginModel);
    Task Logout();
    // Task<RegisterResult> Register(RegisterModel registerModel);
}

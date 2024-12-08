using Microsoft.AspNetCore.Identity;
using Project_Manager.ViewModels;

namespace Project_Manager.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> SignInAsync(LoginVM model);
        Task<IEnumerable<IdentityError>?> RegisterAsync(RegisterVM model);
        Task LogoutAsync();
    }
}

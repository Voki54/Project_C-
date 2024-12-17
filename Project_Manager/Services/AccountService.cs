using Microsoft.AspNetCore.Identity;
using Project_Manager.Models;
using Project_Manager.Services.Interfaces;
using Project_Manager.ViewModels;

namespace Project_Manager.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> SignInAsync(LoginVM model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username!, model.Password!, model.RememberMe, false);
            return result.Succeeded;
        }

        public async Task<IEnumerable<IdentityError>?> RegisterAsync(RegisterVM model)
        {
            AppUser user = new()
            {
                UserName = model.Name,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password!);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return null;
            }
            return result.Errors;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}

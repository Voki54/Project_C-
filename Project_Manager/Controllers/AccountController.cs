using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Models;
using Project_Manager.Services.Interfaces;
using Project_Manager.ViewModels;

namespace Project_Manager.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM model)
    {
        if (ModelState.IsValid)
        {
            if (await _accountService.SignInAsync(model))
            {
                return RedirectToAction("Index", "Projects");
            }

            ModelState.AddModelError("", "Неверное имя или пароль");
        }
        return View(model);
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM model)
    {
        if (ModelState.IsValid)
        {
            var registerErrors = await _accountService.RegisterAsync(model);

            if (registerErrors == null)
            {
                return RedirectToAction("Index", "Projects");
            }

            foreach (var error in registerErrors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _accountService.LogoutAsync();
        return RedirectToAction("Index","Home");
    }
}

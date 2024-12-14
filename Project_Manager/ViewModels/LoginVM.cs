using System.ComponentModel.DataAnnotations;

namespace Project_Manager.ViewModels;

public class LoginVM
{
    [Required(ErrorMessage = "Имя пользователя обязательно.")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Пароль обязателен.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Display(Name = "Remember Me")]
    public bool RememberMe { get; set; }
}

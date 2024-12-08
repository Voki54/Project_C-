using System.ComponentModel.DataAnnotations;

namespace Project_Manager.ViewModels;

public class LoginVM
{
    [Required(ErrorMessage = "Требуется ввести имя.")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Требуется ввести пароль.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    public bool RememberMe { get; set; }
}

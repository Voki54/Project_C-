using System.ComponentModel.DataAnnotations;

namespace Project_Manager.ViewModels;

public class RegisterVM
{
    [Required(ErrorMessage = "Требуется ввести имя.")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Требуется ввести адрес электронной почты.")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Требуется ввести пароль.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Требуется подтвердить пароль.")]
    [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
    [DataType(DataType.Password)]
    public string? ConfirmPassword { get; set; }
}

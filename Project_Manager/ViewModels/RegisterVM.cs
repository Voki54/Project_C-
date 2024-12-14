using System.ComponentModel.DataAnnotations;

namespace Project_Manager.ViewModels;

public class RegisterVM
{
    [Required]
    public string? Name { get; set; }
    [Required]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
    [Display(Name = "Confirm Password")]
    [DataType(DataType.Password)]
    public string? ConfirmPassword { get; set; }
}

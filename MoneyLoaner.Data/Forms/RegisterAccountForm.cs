using System.ComponentModel.DataAnnotations;

namespace MoneyLoaner.Data.Forms;

public class RegisterAccountForm
{
    [Required(ErrorMessage = "To pole jest wymagane...")]
    [StringLength(11)]
    public string? PersonalNumber { get; set; }

    [Required(ErrorMessage = "To pole jest wymagane...")]
    [EmailAddress]
    public string? Email { get; set; }

    [Required(ErrorMessage = "To pole jest wymagane...")]
    [StringLength(30, ErrorMessage = "Hasło powinno mieć conajmniej 8 znaków", MinimumLength = 8)]
    public string? Password { get; set; }

    [Required(ErrorMessage = "To pole jest wymagane...")]
    [Compare(nameof(Password), ErrorMessage = "Podane hasła nie są identyczne...")]
    public string? Password2 { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace MoneyLoaner.Domain.Forms;

public class UpdatePasswordForm
{
    public int UserAccountId { get; set; }

    [Required(ErrorMessage = "To pole jest wymagane...")]
    public string? OldPassword { get; set; }

    [Required(ErrorMessage = "To pole jest wymagane...")]
    [StringLength(30, ErrorMessage = "Hasło powinno mieć conajmniej 8 znaków", MinimumLength = 8)]
    public string? Password { get; set; }

    [Required(ErrorMessage = "To pole jest wymagane...")]
    [Compare(nameof(Password), ErrorMessage = "Podane hasła nie są identyczne...")]
    public string? Password2 { get; set; }
}
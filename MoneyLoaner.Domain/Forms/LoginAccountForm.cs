using System.ComponentModel.DataAnnotations;

namespace MoneyLoaner.Domain.Forms;

public class LoginAccountForm
{
    [Required(ErrorMessage = "To pole jest wymagane...")]
    [StringLength(11, ErrorMessage = "Niepoprawny numer pesel")]
    public string? PersonalNumber { get; set; }

    [Required(ErrorMessage = "To pole jest wymagane...")]
    [EmailAddress]
    public string? Email { get; set; }

    [Required(ErrorMessage = "To pole jest wymagane...")]
    [StringLength(30)]
    public string? Password { get; set; }
}
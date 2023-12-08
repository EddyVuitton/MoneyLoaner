using System.ComponentModel.DataAnnotations;

namespace MoneyLoaner.Data.Forms;

public class ApplicationForm
{
    [Required(ErrorMessage = "Pole nie może być puste..."), StringLength(100, ErrorMessage = "Zbyt długa nazwa...")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Pole nie może być puste..."), StringLength(100, ErrorMessage = "Zbyt długa nazwa...")]
    public string? Surname { get; set; }

    [Required(ErrorMessage = "Pole nie może być puste...")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Pole nie może być puste...")]
    public string? PersonalNumber { get; set; }

    [Required(ErrorMessage = "Pole nie może być puste..."), StringLength(100, ErrorMessage = "Zbyt długi adres email..."), EmailAddress(ErrorMessage = "Nie poprawny adres email...")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Proszę podać kwotę..."), Range(1, int.MaxValue, ErrorMessage = "Kwota powinna wynosić conajmniej 1 zł...")]
    public int? MonthlyIncome { get; set; }

    [Required(ErrorMessage = "Proszę podać kwotę..."), Range(1, int.MaxValue, ErrorMessage = "Kwota powinna wynosić conajmniej 1 zł...")]
    public int? MonthlyExpenses { get; set; }

    [Required(ErrorMessage = "Pole nie może być puste..."), StringLength(32, MinimumLength = 32, ErrorMessage = "Podaj numer 26 cyfr...")]
    public string? CCNumber { get; set; }
}
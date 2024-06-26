﻿using System.ComponentModel.DataAnnotations;

namespace MoneyLoaner.Domain.Forms;

public class RegisterAccountForm
{
    [Required(ErrorMessage = "Pole nie może być puste..."), StringLength(100, ErrorMessage = "Zbyt długa nazwa...")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Pole nie może być puste..."), StringLength(100, ErrorMessage = "Zbyt długa nazwa...")]
    public string? Surname { get; set; }

    [Required(ErrorMessage = "Pole nie może być puste..."), StringLength(11, ErrorMessage = "Podaj prawidłowy numer PESEL...", MinimumLength = 11)]
    public string? PersonalNumber { get; set; }

    [Required(ErrorMessage = "To pole jest wymagane...")]
    [EmailAddress]
    public string? Email { get; set; }

    [Required(ErrorMessage = "To pole jest wymagane..."), StringLength(30, ErrorMessage = "Hasło powinno mieć conajmniej 8 znaków", MinimumLength = 8)]
    public string? Password { get; set; }

    [Required(ErrorMessage = "To pole jest wymagane..."), Compare(nameof(Password), ErrorMessage = "Podane hasła nie są identyczne...")]
    public string? Password2 { get; set; }
}
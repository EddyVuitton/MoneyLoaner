namespace MoneyLoaner.Data.DTOs;

public class UserAccountDto
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public DateTime DateOfCreate { get; set; }
    public bool IsActive { get; set; }
    public int LoanCustomerId { get; set; }
}
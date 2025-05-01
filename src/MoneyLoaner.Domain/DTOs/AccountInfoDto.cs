namespace MoneyLoaner.Domain.DTOs;

public class AccountInfoDto
{
    public string? ClientNumber { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? PersonalNumber { get; set; }
    public DateTime AccountCreateDate { get; set; }
    public int LoanId { get; set; }
    public string? LoanNumber { get; set; }
    public string? CCNumberToRepayment { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}
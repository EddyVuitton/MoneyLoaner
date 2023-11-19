namespace MoneyLoaner.Data.DTOs;

public class ProposalDto
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? PhoneNumber { get; set; }
    public string? PersonalNumber { get; set; }
    public string? Email { get; set; }
    public int? MonthlyIncome { get; set; }
    public int? MonthlyExpenses { get; set; }
    public string? CCNumber { get; set; }
}
namespace MoneyLoaner.Data.DTOs;

public class LoanConfig
{
    public decimal Amount { get; set; }
    public decimal AmountMin { get; set; }
    public decimal AmountMax { get; set; }
    public decimal AmountStep { get; set; }
    public decimal Period { get; set; }
    public decimal PeriodMin { get; set; }
    public decimal PeriodMax { get; set; }
    public decimal PeriodStep { get; set; }
    public decimal Fee { get; set; }
    public decimal InterestRate { get; set; }
}
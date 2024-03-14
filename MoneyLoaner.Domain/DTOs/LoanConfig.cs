namespace MoneyLoaner.Data.DTOs;

public class LoanConfig
{
    public decimal Amount { get; set; }
    public decimal AmountMin { get; set; }
    public decimal AmountMax { get; set; }
    public decimal AmountStep { get; set; }
    public int Period { get; set; }
    public int PeriodMin { get; set; }
    public int PeriodMax { get; set; }
    public int PeriodStep { get; set; }
    public decimal Fee { get; set; }
    public decimal ContractualInterest { get; set; }
    public decimal PenaltyInterest { get; set; }
}
namespace MoneyLoaner.Data.DTOs;

public class LoanDto
{
    public DateTime StartDate { get; set; }
    public DateTime FirstInstallmentPaymentDate { get; set; }
    public int DayOfDatePayment { get; set; }
    public int Installments { get; set; }
    public decimal Principal { get; set; }
    public decimal Fee { get; set; }
    public decimal InterestRate { get; set; }
}
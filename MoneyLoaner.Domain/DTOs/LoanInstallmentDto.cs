namespace MoneyLoaner.Data.DTOs;

public class LoanInstallmentDto
{
    public int PorId { get; set; }
    public int Number { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal Debt { get; set; }
    public decimal Repayment { get; set; }
    public decimal Balance { get; set; }
    public decimal Principal { get; set; }
    public decimal AdmissionFee { get; set; }
    public decimal ContractualInterest { get; set; }
}
namespace MoneyLoaner.Data.DTOs;

public class InstallmentDto
{
    public int Number { get; set; }
    public decimal Principal { get; set; }
    public decimal Interest { get; set; }
    public decimal Fee { get; set; }
    public DateTime PaymentDate { get; set; }

    public decimal Total
    {
        get
        {
            return Principal + Fee + Interest;
        }
        set { }
    }
}
using MoneyLoaner.Data.DTOs;
using System.Xml.Serialization;

namespace MoneyLoaner.WebAPI.Helpers;

public static class LoanHelper
{
    #region PrivateMethods

    private static decimal CalculateXIRREquation(List<InstallmentDto> installments, decimal rate)
    {
        double result = 0;

        for (int i = 0; i < installments.Count; i++)
        {
            var days = (installments[i].PaymentDate - installments[0].PaymentDate).TotalDays;
            result += Convert.ToDouble(installments[i].Total) / Math.Pow(1 + Convert.ToDouble(rate), days / 365);
        }

        return Convert.ToDecimal(result);
    }

    private static decimal CalculateXIRREquationDerivative(List<InstallmentDto> installments, decimal rate)
    {
        double result = 0;

        for (int i = 0; i < installments.Count; i++)
        {
            var days = (installments[i].PaymentDate - installments[0].PaymentDate).TotalDays;
            result -= days * Convert.ToDouble(installments[i].Total) / Math.Pow(1 + Convert.ToDouble(rate), (days / 365) + 1);
        }

        return Convert.ToDecimal(result);
    }

    private static DateTime NextInstallmentDatePayment(DateTime date, LoanDto loan)
    {
        var nextMonth = date.AddMonths(1);
        var endOfMonth = DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month);

        if (loan.DayOfDatePayment > endOfMonth)
        {
            return new DateTime(nextMonth.Year, nextMonth.Month, endOfMonth);
        }

        return new DateTime(nextMonth.Year, nextMonth.Month, loan.DayOfDatePayment);
    }

    #endregion PrivateMethods

    #region PublicMethods

    public static decimal GetFixedInstallmentAmount(LoanDto loan)
    {
        var monthlyInterestRate = loan.InterestRate / 12;

        var fixedInstallmentAmount = Math.Round(
            (double)loan.Principal *
            Math.Pow((double)monthlyInterestRate + 1, loan.Installments) *
            ((double)monthlyInterestRate / (Math.Pow((double)monthlyInterestRate + 1, loan.Installments) - 1))
        , 2);

        return Math.Round((decimal)fixedInstallmentAmount, 2);
    }

    public static List<InstallmentDto> GetInstallmentList(LoanDto loan)
    {
        List<InstallmentDto> _installments = new();
        var fixedInstallmentAmount = GetFixedInstallmentAmount(loan);

        var remainingPrincipalToRepay = loan.Principal;
        var remainingFeeToRepay = Math.Round(loan.Fee, 2);
        var feeAmount = Math.Round(remainingFeeToRepay / loan.Installments, 2);

        var previousPaymentDate = loan.StartDate;
        var currentPaymentDate = loan.FirstInstallmentPaymentDate;

        for (int i = 1; i <= loan.Installments; i++)
        {
            var days = (currentPaymentDate - previousPaymentDate).Days;
            var interestInstallment = Math.Round(remainingPrincipalToRepay * loan.InterestRate / 365 * days, 2);
            var principalInstallment = Math.Round(fixedInstallmentAmount - interestInstallment, 2);

            if (i == loan.Installments)
            {
                principalInstallment = Math.Round(remainingPrincipalToRepay, 2);
                feeAmount = Math.Round(remainingFeeToRepay, 2);
            }

            _installments.Add(new InstallmentDto()
            {
                Number = i,
                Principal = principalInstallment,
                Interest = interestInstallment,
                Fee = feeAmount,
                PaymentDate = currentPaymentDate
            });

            remainingPrincipalToRepay -= Math.Round(principalInstallment, 2);
            remainingFeeToRepay -= Math.Round(feeAmount, 2);
            previousPaymentDate = currentPaymentDate;
            currentPaymentDate = NextInstallmentDatePayment(currentPaymentDate, loan);
        }

        return _installments;
    }

    public static decimal GetAPR(LoanDto loan)
    {
        decimal annualInterestRate = loan.InterestRate / 100;

        decimal rrso = (decimal)Math.Pow((double)(1 + annualInterestRate / 12), loan.Installments);
        rrso = Math.Round((rrso - 1) * 100, 2);

        return Math.Round(loan.Principal * rrso / loan.Installments, 2);
    }

    public static decimal CalculateXIRR(LoanDto loan)
    {
        decimal x0 = 0.1m; // Initial guess for XIRR
        decimal epsilon = 0.0001m; // Tolerance for the Newton-Raphson method
        int maxIterations = 10000; // Increased number of iterations

        decimal x1 = x0;
        decimal fValue, fDerivative;

        var installments = new List<InstallmentDto>
        {
            new InstallmentDto() { Principal = -(loan.Principal + loan.Fee), PaymentDate = loan.StartDate }
        };
        installments.AddRange(GetInstallmentList(loan));

        for (int i = 0; i < maxIterations; i++)
        {
            fValue = CalculateXIRREquation(installments, x1);
            fDerivative = CalculateXIRREquationDerivative(installments, x1);

            if (Math.Abs(fDerivative) < epsilon)
            {
                throw new Exception("Derivative is close to zero. Cannot continue.");
            }

            x1 -= fValue / fDerivative;

            if (Math.Abs(fValue) < epsilon)
            {
                return Math.Round(x1 * 100, 2);
            }
        }

        throw new Exception("XIRR calculation did not converge within the specified number of iterations.");
    }

    public static string GenerateXmlT<T>(T objectToGenerateXml, string rootAttribute)
    {
        using var stream = new StringWriter();
        var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootAttribute));
        serializer.Serialize(stream, objectToGenerateXml);
        var xml = stream.ToString();

        stream.Close();

        return xml;
    }

    #endregion PublicMethods
}
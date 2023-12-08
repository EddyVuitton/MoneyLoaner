namespace MoneyLoaner.WebAPI.BusinessLogic.Scoring;

public interface IScoringBusinessLogic
{
    Task CalculateScoringAsync(int po_id);
}
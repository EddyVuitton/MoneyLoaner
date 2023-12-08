using MoneyLoaner.Data.Context;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.WebAPI.Data;
using MoneyLoaner.WebAPI.Extensions;
using System.Data;

namespace MoneyLoaner.WebAPI.BusinessLogic.Scoring;

public class ScoringBusinessLogic : IScoringBusinessLogic
{
    private readonly DBContext _context;

    public ScoringBusinessLogic(DBContext context)
    {
        _context = context;
    }

    public async Task CalculateScoringAsync(int po_id)
    {
        var hT = new object[]
        {
            SqlParam.CreateParameter("po_id", po_id, SqlDbType.Int)
        };

        await _context.SqlQueryAsync<LoanInstallmentDto>("exec p_scoring_wylicz @po_id;", hT);
    }
}
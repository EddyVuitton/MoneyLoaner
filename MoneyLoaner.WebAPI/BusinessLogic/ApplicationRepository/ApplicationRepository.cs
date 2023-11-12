using MoneyLoaner.Data.Context;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.WebAPI.Extensions;

namespace MoneyLoaner.WebAPI.BusinessLogic.ApplicationRepository;

public class ApplicationRepository : IApplicationRepository
{
    private readonly DBContext _context;

    public ApplicationRepository(DBContext context)
    {
        _context = context;
    }

    public async Task<TestModelDto> GetTest()
    {
        var result = await _context.SqlQueryAsync<TestModelDto>("select 1 as id");
        
        return result.FirstOrDefault();
    }
}
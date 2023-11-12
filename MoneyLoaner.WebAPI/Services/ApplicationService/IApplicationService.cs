using DebtWeb.WebAPI.Data;
using MoneyLoaner.Data.DTOs;

namespace MoneyLoaner.WebAPI.Services.ApplicationService;

public interface IApplicationService
{
    Task<HttpResponse<TestModelDto>> GetTest();
}
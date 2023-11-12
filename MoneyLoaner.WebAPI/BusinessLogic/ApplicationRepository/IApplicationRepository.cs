using MoneyLoaner.Data.DTOs;

namespace MoneyLoaner.WebAPI.BusinessLogic.ApplicationRepository;

public interface IApplicationRepository
{
    Task<TestModelDto> GetTest();
}
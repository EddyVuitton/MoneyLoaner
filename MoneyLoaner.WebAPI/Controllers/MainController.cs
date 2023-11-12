using DebtWeb.WebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using MoneyLoaner.Data.DTOs;
using MoneyLoaner.WebAPI.BusinessLogic.ApplicationRepository;
using MoneyLoaner.WebAPI.Helpers;

namespace MoneyLoaner.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MainController : ControllerBase
{
    private readonly ILogger<MainController> _logger;
    private readonly IApplicationRepository _applicationRepository;
    private readonly IConfiguration _configuration;

    public MainController(ILogger<MainController> logger, IApplicationRepository applicationRepository, IConfiguration configuration)
    {
        _logger = logger;
        _applicationRepository = applicationRepository;
        _configuration = configuration;
    }

    [HttpGet("GetTest")]
    public async Task<HttpResponse<TestModelDto>> GetTest()
    {
        try
        {
            var result = await _applicationRepository.GetTest();
            return HttpHelper.Ok(result);
        }
        catch (Exception e)
        {
            return HttpHelper.Error<TestModelDto>(e);
        }
    }
}
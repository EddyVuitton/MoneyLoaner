using Microsoft.AspNetCore.Mvc;

namespace MoneyLoaner.WebAPI.BusinessLogic.Scoring;

[ApiController]
[Route("api/[controller]")]
public class ScoringController : ControllerBase
{
    private readonly ILogger<ScoringController> _logger;
    private readonly IScoringBusinessLogic _businessLogic;
    private readonly IConfiguration _configuration;

    public ScoringController(ILogger<ScoringController> logger, IScoringBusinessLogic businessLogic, IConfiguration configuration)
    {
        _logger = logger;
        _businessLogic = businessLogic;
        _configuration = configuration;
    }
}
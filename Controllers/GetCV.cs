// Route en get pour recuperer une chaine cv
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace generate_pages_ia.Controllers;

[ApiController]
[Route("[controller]")]
public class GetCV : ControllerBase
{
    private readonly ILogger<GetCV> _logger;

    public GetCV(ILogger<GetCV> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetCV")]
    public string Get()
    {
        return "CV";
    }
}

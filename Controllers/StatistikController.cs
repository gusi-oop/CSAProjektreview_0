using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace CSAProjectReview.Controllers;

[ApiController]
[Route("[controller]")]
public class StatistikController : ControllerBase
{
    private readonly ILogger<StatistikController> _logger;

    public StatistikController(ILogger<StatistikController> logger)
    {
        _logger = logger;
    }   

    [HttpGet(Name = "GetAufrufStatistik")]
       public IEnumerable<AufrufStatistik> Get()
       {

        return Enumerable.Range(1, 1).Select(index => new AufrufStatistik
        {
            Stadt = "Berlin",
            Aufrufe = 23
        }).ToArray();
           
}
}

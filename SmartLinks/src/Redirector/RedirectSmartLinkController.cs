using Microsoft.AspNetCore.Mvc;

namespace Redirector;

[ApiController]
[Route("{*slug}")]
public class RedirectSmartLinkController() : ControllerBase
{
    [HttpGet]
    [HttpHead]
    public ActionResult Get(string slug)
    {   
        return Ok();
    }
}
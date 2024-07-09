using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using server.Controllers.ObjectResults;

namespace Uds.Controllers;

public class UdsController : ControllerBase
{
    protected string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
    protected string UserRole => User.FindFirstValue(ClaimTypes.Role);

    [NonAction]
    public override OkObjectResult Ok(object? value)
    {
        return base.Ok(value);
    }

    [NonAction]
    public ServerErrorObjectResult ServerError(string optionalMessage = null) =>
        new ServerErrorObjectResult(new ServerErrorModel(optionalMessage));
}

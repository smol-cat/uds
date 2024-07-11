using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using server.Controllers.ObjectResults;
using Uds.Repositories;

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
    protected IActionResult CommitedChangesResult(IActionResult defaultResult, BaseRepository respository)
    {
        return respository.TrySaveChanges() ? defaultResult : ServerError("Error occured while trying to save changes to the database");
    }

    [NonAction]
    public ServerErrorObjectResult ServerError(string optionalMessage = null) =>
        new ServerErrorObjectResult(new ServerErrorModel(optionalMessage));
}

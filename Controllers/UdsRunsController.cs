using Microsoft.AspNetCore.Mvc;
using server.Controllers.ObjectResults;
using Uds.Models;
using Uds.Models.Database;
using Uds.Models.Request;
using Uds.Repositories;

namespace Uds.Controllers;

[ApiController]
[Route("/api/uds/runs")]
public class UdsRunsController : UdsController
{
    private UdsOrdersRepository _udsOrdersRepository;
    private UdsRunsRepository _udsRunsRepository;

    public UdsRunsController(UdsOrdersRepository udsOrdersRepository, UdsRunsRepository udsRunsRepository)
    {
        _udsOrdersRepository = udsOrdersRepository;
        _udsRunsRepository = udsRunsRepository;
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult Get(int id)
    {
        var udsRun = _udsRunsRepository.GetValidUdsRun(id);
        if (udsRun == null)
        {
            return NotFound(new ServerErrorModel("Order run was not found"));
        }

        return Ok();
    }

    [HttpGet]
    public IActionResult GetList()
    {
        return Ok(_udsRunsRepository.GetUdsRuns());
    }

    [HttpPatch]
    public IActionResult PatchOrderRun(UdsRunPatchModel patchModel)
    {
        UdsRunModel runModel = _udsRunsRepository.GetValidUdsRun(patchModel.Id);
        if (runModel == null)
        {
            return NotFound(new ServerErrorModel("Order run to restart was not found"));
        }

        if (!_udsRunsRepository.TryPatchRun(runModel, patchModel))
        {
            return ServerError("Error occured while restarting the uds run");
        }

        return CommitedChangesResult(Ok(runModel), _udsRunsRepository);
    }

    [HttpPost]
    public IActionResult StartOrder(UdsRunStartModel runModel)
    {
        UdsOrderModel orderModel = _udsOrdersRepository.GetOrder(runModel.OrderId);
        if (orderModel == null)
        {
            return NotFound(new ServerErrorModel("Order to start the run for was not found"));
        }

        if (!_udsRunsRepository.TryStartRun(runModel, out UdsRunModel udsRunModel))
        {
            return ServerError("Error occured while starting the uds run");
        }

        IActionResult result = CommitedChangesResult(Ok(), _udsRunsRepository);
        if (result is ServerErrorObjectResult)
        {
            return result;
        }

        UdsRunModel createdRun = _udsRunsRepository.GetUdsRun(udsRunModel.Id);
        if (createdRun == null)
        {
            return ServerError("Failed to create a uds run");
        }

        return Ok(createdRun);
    }
}

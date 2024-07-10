using Microsoft.AspNetCore.Mvc;
using Uds.Models;
using Uds.Repositories;

namespace Uds.Controllers;

[ApiController]
[Route("/api/udsRuns")]
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
        return Ok(_udsRunsRepository.GetUdsRun(id));
    }

    [HttpGet]
    public IActionResult GetList()
    {
        return Ok(_udsRunsRepository.GetUdsRuns());
    }

    [HttpPatch]
    [Route("{id}")]
    public IActionResult RestartOrder(int id)
    {
        UdsRunModel runModel = _udsRunsRepository.GetUdsRun(id);
        if (runModel == null)
        {
            return NotFound(new ServerErrorModel("Order run to restart was not found"));
        }

        if (!_udsRunsRepository.TryRestartRun(ref runModel))
        {
            return ServerError("Error occured while restarting the uds run");
        }

        return Ok(runModel);
    }

    [HttpPost]
    public IActionResult StartOrder(UdsRunStartModel runModel)
    {
        UdsOrderModel orderModel = _udsOrdersRepository.GetOrder(runModel.OrderId);
        if (orderModel == null)
        {
            return NotFound(new ServerErrorModel("Order run to restart was not found"));
        }

        if (!_udsRunsRepository.TryStartRun(runModel))
        {
            return ServerError("Error occured while restarting the uds run");
        }

        return Ok(runModel);
    }
}

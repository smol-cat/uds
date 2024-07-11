using Microsoft.AspNetCore.Mvc;
using Uds.Repositories;

namespace Uds.Controllers;

[ApiController]
[Route("/api/udsOrders/{orderId}/udsRuns")]
public class UdsOrderRunsController : UdsController
{
    private UdsOrdersRepository _ordersRepository;
    private UdsRunsRepository _runsRepository;

    public UdsOrderRunsController(UdsOrdersRepository ordersRepository, UdsRunsRepository runsRepository)
    {
        _ordersRepository = ordersRepository;
        _runsRepository = runsRepository;
    }

    [HttpGet]
    public IActionResult GetList(int orderId)
    {
        if (_ordersRepository.GetOrder(orderId) == null)
        {
            return NotFound(new ServerErrorModel("Order was not found"));
        }

        return Ok(_runsRepository.GetValidUdsOrderRuns(orderId));
    }
}

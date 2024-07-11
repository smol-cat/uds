using Microsoft.AspNetCore.Mvc;
using Uds.Models;
using Uds.Models.Database;
using Uds.Models.Request;
using Uds.Repositories;

namespace Uds.Controllers;

[ApiController]
[Route("/api/udsOrders")]
public class UdsOrdersController : UdsController
{
    private UdsOrdersRepository _ordersRepository;
    private UdsRunsRepository _runsRepository;

    public UdsOrdersController(UdsOrdersRepository ordersRepository, UdsRunsRepository runsRepository)
    {
        _ordersRepository = ordersRepository;
        _runsRepository = runsRepository;
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult Get(int id)
    {
        UdsOrderModel order = _ordersRepository.GetOrder(id);
        if (order == null)
        {
            return NotFound(new ServerErrorModel("Order was not found"));
        }

        return Ok(order);
    }

    [HttpGet]
    public IActionResult GetList()
    {
        return Ok(_ordersRepository.GetUdsOrders());
    }

    [HttpPost]
    public IActionResult CreateOrder(UdsOrderCreateModel order)
    {
        if (!_ordersRepository.TryCreateOrder(order))
        {
            return ServerError("Could not create an order");
        }

        return CommitedChangesResult(NoContent(), _ordersRepository);
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult DeleteOrder(int id)
    {
        UdsOrderModel orderToDelete = _ordersRepository.GetOrder(id);
        if (orderToDelete == null)
        {
            return NotFound(new ServerErrorModel("Order was not found"));
        }

        if (!_ordersRepository.TryDeleteOrder(orderToDelete))
        {
            return ServerError("Failed to delete an order");
        }

        List<UdsRunModel> runs = _runsRepository.GetValidUdsOrderRuns(id);
        if (!_runsRepository.TryDeleteRuns(runs))
        {
            return ServerError("Failed to delete order runs");
        }

        return CommitedChangesResult(NoContent(), _ordersRepository);
    }
}

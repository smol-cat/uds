using Microsoft.AspNetCore.Mvc;
using Uds.Models;
using Uds.Models.Database;
using Uds.Models.Request;
using Uds.Repositories;

namespace Uds.Controllers;

[ApiController]
[Route("/api/uds/orders")]
public class UdsOrdersController : UdsController
{
    private UdsOrdersRepository _ordersRepository;
    private UdsRunsRepository _runsRepository;
    private BookingSitesRepository _bookingSitesRepository;
    private ScheduleRepository _scheduleRepository;

    public UdsOrdersController(
            UdsOrdersRepository ordersRepository,
            UdsRunsRepository runsRepository,
            BookingSitesRepository bookingSitesRepository,
            ScheduleRepository scheduleRepository)
    {
        _ordersRepository = ordersRepository;
        _runsRepository = runsRepository;
        _bookingSitesRepository = bookingSitesRepository;
        _scheduleRepository = scheduleRepository;
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
        if (!_bookingSitesRepository.BookingSiteExists(order.BookingSiteId))
        {
            return NotFound(new ServerErrorModel("Booking site specified was not found"));
        }

        if (!_scheduleRepository.ScheduleExists(order.ScheduleId))
        {
            return NotFound(new ServerErrorModel("Schedule specified was not found"));
        }

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

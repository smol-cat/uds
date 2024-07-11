using Uds.Database;
using Uds.Models;
using Uds.Models.Request;

namespace Uds.Repositories;

public class UdsOrdersRepository : BaseRepository
{
    public UdsOrdersRepository(DbConnection db) : base(db)
    {
    }

    private IQueryable<UdsOrderModel> Extend(IQueryable<UdsOrderModel> other)
    {
        return from order in other
               join bookingSite in _db.BookingSites on order.BookingSiteId equals bookingSite.Id
               join schedule in _db.Schedules on order.ScheduleId equals schedule.Id
               select new UdsOrderModel
               {
                   Id = order.Id,
                   ProfileId = order.ProfileId,
                   ScheduleId = order.ScheduleId,
                   BookingSiteId = order.BookingSiteId,
                   Origin = order.Origin,
                   Destination = order.Destination,
                   BookingSite = bookingSite,
                   Schedule = schedule,
               };
    }

    public List<UdsOrderModel> GetUdsOrders()
    {
        return [.. Extend(_db.UdsOrders.Where(e => e.Deleted == false))];
    }

    public UdsOrderModel GetOrder(int id)
    {
        return Extend(_db.UdsOrders.Where(e => e.Id == id && e.Deleted == false)).FirstOrDefault();
    }

    public bool TryDeleteOrder(UdsOrderModel orderToDelete)
    {
        orderToDelete.Deleted = true;
        _db.UdsOrders.Update(orderToDelete);
        return true;
    }

    public bool TryCreateOrder(UdsOrderCreateModel order)
    {
        _db.UdsOrders.Add(new UdsOrderModel(order));
        return true;
    }
}

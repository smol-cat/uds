using Uds.Database;
using Uds.Models;
using Uds.Models.Request;

namespace Uds.Repositories;

public class UdsOrdersRepository : BaseRepository
{
    public UdsOrdersRepository(DbConnection db) : base(db)
    {
    }

    public List<UdsOrderModel> GetUdsOrders()
    {
        return _db.UdsOrders.Where(e => e.Deleted == false).ToList();
    }

    public UdsOrderModel GetOrder(int id)
    {
        return _db.UdsOrders.Where(e => e.Id == id && e.Deleted == false).FirstOrDefault();
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

using Uds.Database;
using Uds.Models;

namespace Uds.Repositories;

public interface IUdsOrderRepository
{
    List<UdsOrderModel> GetUdsOrders();
}

public class UdsOrdersRepository : IUdsOrderRepository
{
    private DbConnection _db;

    public UdsOrdersRepository(DbConnection db)
    {
        _db = db;
    }

    public List<UdsOrderModel> GetUdsOrders()
    {
        return _db.UdsOrders.Where(e => e.Deleted == false).ToList();
    }

    public UdsOrderModel GetOrder(int id)
    {
        return _db.UdsOrders.Where(e => e.Id == id).FirstOrDefault();
    }

    public bool TryDeleteOrder(UdsOrderModel orderToDelete)
    {
        orderToDelete.Deleted = true;
        _db.UdsOrders.Update(orderToDelete);
        return _db.TrySaveChanges();
    }

    public bool TryCreateOrder(UdsOrderModel order)
    {
        try
        {
            _db.UdsOrders.Add(order);
            _db.SaveChanges();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }
}

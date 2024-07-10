using Uds.Database;
using Uds.Models;

namespace Uds.Repositories;

public class UdsRunsRepository
{
    private DbConnection _db;
    private UdsOrdersRepository _ordersRepository;

    public UdsRunsRepository(DbConnection db, UdsOrdersRepository ordersRepository)
    {
        _db = db;
        _ordersRepository = ordersRepository;
    }

    private IQueryable<UdsRunModel> JoinWithStatusDescription(IQueryable<UdsRunModel> udsRunQuery)
    {
        return udsRunQuery.Join(_db.StatusModels,
                udsRun => udsRun.StatusId,
                status => status.Id,
                (udsRun, status) => new UdsRunModel()
                {
                    Id = udsRun.Id,
                    OrderId = udsRun.OrderId,
                    StartTime = udsRun.StartTime,
                    EndTime = udsRun.EndTime,
                    StatusDescription = status.Description,
                }
             );
    }

    public List<UdsRunModel> GetUdsRuns()
    {
        return [.. JoinWithStatusDescription(_db.UdsRuns.Where(e => e.EndTime > DateTime.Now))];
    }

    public List<UdsRunModel> GetUdsOrderRuns(int orderId)
    {
        return [.. JoinWithStatusDescription(_db.UdsRuns.Where(e => e.EndTime > DateTime.Now && e.OrderId == orderId))];
    }

    public UdsRunModel GetValidUdsRun(int id)
    {
        return JoinWithStatusDescription(_db.UdsRuns.Where(e => e.EndTime > DateTime.Now && e.Id == id))
            .FirstOrDefault();
    }

    public bool TryRestartRun(ref UdsRunModel runModel)
    {
        runModel.StartTime = DateTime.Now;
        runModel.EndTime = DateTimeOffset.Now.AddDays(1).DateTime;
        runModel.StatusId = (int)StatusDecription.Active;

        _db.UdsRuns.Update(runModel);
        return _db.TrySaveChanges();
    }

    public bool TryCancelRuns(List<UdsRunModel> modelsToCancel)
    {
        modelsToCancel.ForEach(e => e.StatusId = (int)StatusDecription.Canceled);
        _db.UdsRuns.UpdateRange(modelsToCancel);
        return _db.TrySaveChanges();
    }

    public bool TryStartRun(UdsRunStartModel runStartModel)
    {
        UdsRunModel runModel = new()
        {
            OrderId = runStartModel.OrderId,
            StartTime = DateTime.Now,
            EndTime = DateTimeOffset.Now.AddDays(1).DateTime,
            StatusId = (int)StatusDecription.Active
        };

        _db.UdsRuns.Add(runModel);
        return _db.TrySaveChanges();
    }
}

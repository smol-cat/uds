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

    public List<UdsRunModel> GetUdsRuns()
    {
        return _db.UdsRuns
            .Join(_db.StatusModels,
                    udsRun => udsRun.StatusId,
                    status => status.Id,
                    (udsRun, status) => new UdsRunModel()
                    {
                        Id = udsRun.Id,
                        OrderId = udsRun.OrderId,
                        StartTime = udsRun.StartTime,
                        EndTime = udsRun.EndTime,
                        Status = status.Description,
                    }
                 )
            .Where(e => e.StatusId == (int)StatusDecription.Active && e.EndTime > DateTime.Now)
            .ToList();
    }

    public UdsRunModel GetUdsRun(int id)
    {
        return _db.UdsRuns.Where(e => e.Id == id).FirstOrDefault();
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

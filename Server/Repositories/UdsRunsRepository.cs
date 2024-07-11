using Uds.Database;
using Uds.Models;
using Uds.Models.Database;
using Uds.Models.Request;

namespace Uds.Repositories;

public class UdsRunsRepository : BaseRepository
{
    private UdsOrdersRepository _ordersRepository;

    public UdsRunsRepository(DbConnection db, UdsOrdersRepository ordersRepository) : base(db)
    {
        _ordersRepository = ordersRepository;
    }

    private IQueryable<UdsRunModel> JoinWithStatusDescription(IQueryable<UdsRunModel> udsRunQuery)
    {
        return udsRunQuery
            .Where(e => !e.Deleted)
            .Join(_db.Statuses,
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
             );
    }

    public List<UdsRunModel> GetUdsRuns()
    {
        return [.. JoinWithStatusDescription(_db.UdsRuns)];
    }

    public List<UdsRunModel> GetValidUdsOrderRuns(int orderId)
    {
        return [.. JoinWithStatusDescription(_db.UdsRuns.Where(e => e.EndTime > DateTime.Now && e.OrderId == orderId))];
    }

    public UdsRunModel GetUdsRun(int id)
    {
        return JoinWithStatusDescription(_db.UdsRuns.Where(e => e.Id == id))
            .FirstOrDefault();
    }

    public UdsRunModel GetValidUdsRun(int id)
    {
        return JoinWithStatusDescription(_db.UdsRuns.Where(e => e.EndTime > DateTime.Now && e.Id == id))
            .FirstOrDefault();
    }

    public bool TryPatchRun(UdsRunModel runModel, UdsRunPatchModel patchModel)
    {
        switch (patchModel.Action)
        {
            case OrderAction.Restart:
                runModel.StatusId = (int)StatusDecription.Active;
                runModel.StartTime = DateTime.Now;
                runModel.EndTime = DateTimeOffset.Now.AddDays(1).DateTime;
                break;
            case OrderAction.Cancel:
                runModel.StatusId = (int)StatusDecription.Canceled;
                runModel.EndTime = DateTime.Now;
                break;
            case OrderAction.End:
                runModel.StatusId = (int)StatusDecription.Ended;
                runModel.EndTime = DateTime.Now;
                break;
            default:
                return false;
        }

        _db.UdsRuns.Update(runModel);
        return true;
    }

    public bool TryDeleteRuns(List<UdsRunModel> modelsToCancel)
    {
        modelsToCancel.ForEach(e =>
        {
            e.StatusId = (int)StatusDecription.Canceled;
            e.Deleted = true;
        });

        _db.UdsRuns.UpdateRange(modelsToCancel);
        return true;
    }

    public bool TryStartRun(UdsRunStartModel runStartModel, out UdsRunModel createdRun)
    {
        UdsRunModel runModel = new()
        {
            OrderId = runStartModel.OrderId,
            StartTime = DateTime.Now,
            EndTime = DateTimeOffset.Now.AddDays(1).DateTime,
            StatusId = (int)StatusDecription.Active
        };

        _db.UdsRuns.Add(runModel);
        createdRun = runModel;
        return true;
    }
}

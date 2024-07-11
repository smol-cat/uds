using Uds.Database;

namespace Uds.Repositories;

public class ScheduleRepository : BaseRepository
{
    public ScheduleRepository(DbConnection db) : base(db)
    {
    }

    public bool ScheduleExists(int scheduleId)
    {
        return _db.Schedules.Where(e => e.Id == scheduleId).Any();
    }
}

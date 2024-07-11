using Uds.Database;

namespace Uds.Repositories;

public abstract class BaseRepository
{
    protected DbConnection _db;

    public BaseRepository(DbConnection db)
    {
        _db = db;
    }

    public bool TrySaveChanges()
    {
        return _db.TrySaveChanges();
    }
}

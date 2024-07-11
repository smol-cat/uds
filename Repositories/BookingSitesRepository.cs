using Uds.Database;

namespace Uds.Repositories;

public class BookingSitesRepository : BaseRepository
{
    public BookingSitesRepository(DbConnection db) : base(db)
    {
    }

    public bool BookingSiteExists(int bookingSiteId)
    {
        return _db.BookingSites.Where(e => e.Id == bookingSiteId).Any();
    }
}

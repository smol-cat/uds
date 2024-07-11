using Microsoft.EntityFrameworkCore;
using Uds.Models;
using Uds.Models.Database;

namespace Uds.Database;

public class DbConnection : DbContext
{
    private static string _connectionString;
    private IConfiguration _configuration;

    public DbSet<UdsOrderModel> UdsOrders { get; set; }
    public DbSet<UdsRunModel> UdsRuns { get; set; }
    public DbSet<StatusModel> StatusModels { get; set; }

    public DbConnection(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = $"server={_configuration["DB:host"]};port={_configuration["DB:port"]};database={_configuration["DB:database"]};username={_configuration["DB:username"]};password={_configuration["DB:password"]};";
        ServerVersion serverVersion = ServerVersion.AutoDetect(connectionString);
        optionsBuilder.UseMySql(connectionString, serverVersion);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UdsOrderModel>();
        modelBuilder.Entity<UdsRunModel>().Ignore(e => e.Status);
    }

    public bool TrySaveChanges()
    {
        try
        {
            SaveChanges();
            return true;
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine($"[DATABASE UPDATE ERROR]{e.Message}");
            Console.WriteLine($"{e.InnerException?.Message}");
            Console.WriteLine($"{e.StackTrace}");
            return false;
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace homeMonitor.Models
{
    public class MonitorDbContext : DbContext
    {
        public MonitorDbContext(DbContextOptions<MonitorDbContext> options) : base(options)
        {
        }

        public DbSet<Models.Metric> metrics { get; set; }
    }
}

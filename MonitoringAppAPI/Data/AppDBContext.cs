using Microsoft.EntityFrameworkCore;
using MonitoringAppAPI.Models;

namespace MonitoringAppAPI.Data
{
    public class AppDBContext: DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<MonitoredApp> MonitoredApps { get; set; }
    }
}

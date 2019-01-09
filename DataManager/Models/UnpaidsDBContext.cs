using Microsoft.EntityFrameworkCore;
using UnpaidModels;

namespace DataManager.Models
{
    public class UnpaidsDBContext: DbContext
    {
        public UnpaidsDBContext(DbContextOptions<UnpaidsDBContext> options) : base(options)
        {
        }

        public DbSet<UnpaidDb> Unpaids { get; set; }
        public DbSet<UnpaidRequestDb> UnpaidRequests { get; set; }
        public DbSet<UnpaidResponseDb> UnpaidResponses { get; set; }
    }
}

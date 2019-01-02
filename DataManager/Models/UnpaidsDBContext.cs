using System;
using Microsoft.EntityFrameworkCore;
using UnpaidModels;

namespace DataManager
{
    public class UnpaidsDBContext: DbContext
    {
        public UnpaidsDBContext(DbContextOptions<UnpaidsDBContext> options) : base(options)
        {
        }

        public DbSet<Unpaid> Unpaids { get; set; }
        public DbSet<UnpaidRequest> UnpaidRequests { get; set; }
        public DbSet<UnpaidResponse> UnpaidResponses { get; set; }
    }
}

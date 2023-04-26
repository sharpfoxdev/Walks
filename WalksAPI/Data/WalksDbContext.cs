using Microsoft.EntityFrameworkCore;
using WalksAPI.Models.Domain;

namespace WalksAPI.Data {
    public class WalksDbContext : DbContext
    {
        public WalksDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }
        // sets/collections of entities in a database
        public DbSet<Region> Regions { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Walk> Walks { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using WalksAPI.Data;
using WalksAPI.Models.Domain;

namespace WalksAPI.Repositories {
    public class SQLRegionRepository : IRegionRepository {
        private readonly WalksDbContext dbContext;

        public SQLRegionRepository(WalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Region>> GetAllAsync() {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id) {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Region> CreateAsync(Region region) {
            await dbContext.Regions.AddAsync(region); // tracking changes
            await dbContext.SaveChangesAsync(); // commit changes to the database
            return region;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region) {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if(existingRegion == null) {
                return null;
            }
            // map DTO to domain model
            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.RegionImageUrl = region.RegionImageUrl;

            await dbContext.SaveChangesAsync();
            return existingRegion; // now updated region
        }

        public async Task<Region?> DeleteAsync(Guid id) {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if(existingRegion == null) {
                return null;
            }
            dbContext.Regions.Remove(existingRegion); // delete doesnt have async version
            await dbContext.SaveChangesAsync(); // commit changes to the database
            return existingRegion; // now deleted region
        }
    }
}

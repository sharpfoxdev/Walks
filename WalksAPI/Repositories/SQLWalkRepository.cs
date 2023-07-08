using WalksAPI.Data;
using WalksAPI.Models.Domain;

namespace WalksAPI.Repositories {
    public class SQLWalkRepository : IWalkRepository {
        private readonly WalksDbContext dbContext;

        public SQLWalkRepository(WalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk) {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id) {
            throw new NotImplementedException();
        }

        public async Task<List<Walk>> GetAllAsync() {
            throw new NotImplementedException();
        }

        public async Task<Walk?> GetByIdAsync(Guid id) {
            throw new NotImplementedException();
        }

        public Task<Walk?> UpdateAsync(Guid id, Walk walk) {
            throw new NotImplementedException();
        }
    }
}

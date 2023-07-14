using Microsoft.AspNetCore.Mvc;
using WalksAPI.Models.Domain;

namespace WalksAPI.Repositories {
    public interface IWalkRepository {
        Task<Walk> CreateAsync(Walk walk);
        Task<Walk?> DeleteAsync(Guid id);
        Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null);
        Task<Walk?> GetByIdAsync(Guid id);
        Task<Walk?> UpdateAsync(Guid id, Walk walk);
    }
}

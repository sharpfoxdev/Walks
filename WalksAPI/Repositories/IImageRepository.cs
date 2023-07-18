using WalksAPI.Models.Domain;

namespace WalksAPI.Repositories {
    public interface IImageRepository {
        Task<Image> UploadAsync(Image image);
    }
}

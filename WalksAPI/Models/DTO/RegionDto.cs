namespace WalksAPI.Models.DTO {
    /// <summary>
    /// Tells us, what we should return to the user, we can use just a subset of Region properties
    /// </summary>
    public class RegionDto {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}

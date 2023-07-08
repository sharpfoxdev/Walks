namespace WalksAPI.Models.DTO {
    public class WalkDto {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        //we refference other DTOs from this DTO, not directly the domain models
        public DifficultyDto Difficulty { get; set; }
        public RegionDto Region { get; set; }
    }
}

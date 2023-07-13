using System.ComponentModel.DataAnnotations;

namespace WalksAPI.Models.DTO {
    public class UpdateWalkRequestDto {
        [Required]
        [MaxLength(50, ErrorMessage = "Name has to be at most 50 characters long")]
        public string Name { get; set; }
        [Required]
        [MaxLength(500, ErrorMessage = "Description has to be at most 500 characters long")]
        public string Description { get; set; }
        [Required]
        [Range(0, 1000, ErrorMessage = "Length has to be between 0 and 1000 km")]
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }
    }
}

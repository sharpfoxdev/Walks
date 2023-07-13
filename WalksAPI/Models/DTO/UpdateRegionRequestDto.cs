using System.ComponentModel.DataAnnotations;

namespace WalksAPI.Models.DTO {
    public class UpdateRegionRequestDto {
        [Required]
        [MinLength(3, ErrorMessage = "Code has to at least 3 characters long")]
        [MaxLength(3, ErrorMessage = "Code has to at most 3 characters long")]
        public string Code { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Name has to be at most 50 characters long")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}

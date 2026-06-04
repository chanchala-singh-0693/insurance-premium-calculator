using System.ComponentModel.DataAnnotations;

namespace InsurancePremium.API.Models
{
    
    public class PremiumRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Age Next Birthday is required.")]
        [Range(1, 120, ErrorMessage = "Age Next Birthday must be between 1 and 120.")]
        public int AgeNextBirthday { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{4}$",
            ErrorMessage = "Date of Birth must be in mm/YYYY format (e.g., 06/1990).")]
        public string DateOfBirth { get; set; } = string.Empty;

        [Required(ErrorMessage = "Occupation is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "A valid occupation must be selected.")]
        public int OccupationId { get; set; }

        [Required(ErrorMessage = "Death Sum Insured is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Death Sum Insured must be greater than zero.")]
        public decimal DeathSumInsured { get; set; }
    }

    public class PremiumResponse
    {
        public string MemberName { get; set; } = string.Empty;

        public string OccupationName { get; set; } = string.Empty;

        public string OccupationRating { get; set; } = string.Empty;

        public decimal OccupationFactor { get; set; }

        public decimal MonthlyPremium { get; set; }

        public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;
    }
}

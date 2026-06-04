using InsurancePremium.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace InsurancePremium.Services
{
    public class PremiumCalculationService : IPremiumCalculationService
    {
        private readonly IOccupationRepository _occupationRepository;
        private readonly ILogger<PremiumCalculationService> _logger;

        public PremiumCalculationService(
            IOccupationRepository occupationRepository,
            ILogger<PremiumCalculationService> logger)
        {
            _occupationRepository = occupationRepository;
            _logger = logger;
        }

        public async Task<decimal> CalculateMonthlyPremiumAsync(
            decimal deathSumInsured,
            int occupationId,
            int ageNextBirthday)
        {
            if (deathSumInsured <= 0)
                throw new ArgumentOutOfRangeException(nameof(deathSumInsured),
                    "Death sum insured must be greater than zero.");

            if (ageNextBirthday <= 0 || ageNextBirthday > 120)
                throw new ArgumentOutOfRangeException(nameof(ageNextBirthday),
                    "Age next birthday must be between 1 and 120.");

            _logger.LogInformation(
                "Calculating premium for OccupationId={OccupationId}, Age={Age}, DeathCover={Cover}",
                occupationId, ageNextBirthday, deathSumInsured);

            var factor = await GetOccupationFactorAsync(occupationId);

            var monthlyPremium = (deathSumInsured * factor * ageNextBirthday) / 1000m / 12m;

            var result = Math.Round(monthlyPremium, 2);

            _logger.LogInformation(
                "Premium calculated successfully: {Premium}", result);

            return result;
        }

        public async Task<decimal> GetOccupationFactorAsync(int occupationId)
        {
            var occupation = await _occupationRepository.GetOccupationByIdAsync(occupationId);
            if (occupation == null)
            {
                _logger.LogWarning("Occupation not found for Id={OccupationId}", occupationId);
                throw new ArgumentException($"Occupation with ID {occupationId} was not found.");
            }

            var rating = await _occupationRepository.GetRatingByNameAsync(occupation.Rating);
            if (rating == null)
            {
                _logger.LogError(
                    "Rating '{Rating}' not found for Occupation '{Occupation}'",
                    occupation.Rating, occupation.Name);
                throw new ArgumentException(
                    $"Rating '{occupation.Rating}' for occupation '{occupation.Name}' was not found.");
            }

            return rating.Factor;
        }
    }
}

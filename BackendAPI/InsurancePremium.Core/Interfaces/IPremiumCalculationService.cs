using InsurancePremium.Core.Entities;

namespace InsurancePremium.Core.Interfaces
{
    public interface IPremiumCalculationService
    {
        Task<decimal> CalculateMonthlyPremiumAsync(decimal deathSumInsured, int occupationId, int ageNextBirthday);

        Task<decimal> GetOccupationFactorAsync(int occupationId);
    }
}

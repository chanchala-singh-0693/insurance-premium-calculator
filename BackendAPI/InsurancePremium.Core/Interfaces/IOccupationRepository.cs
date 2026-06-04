using InsurancePremium.Core.Entities;

namespace InsurancePremium.Core.Interfaces
{
    public interface IOccupationRepository
    {
        Task<IEnumerable<Occupation>> GetAllOccupationsAsync();

        Task<Occupation?> GetOccupationByIdAsync(int id);

        Task<IEnumerable<OccupationRating>> GetAllRatingsAsync();
        Task<OccupationRating?> GetRatingByNameAsync(string ratingName);
    }
}

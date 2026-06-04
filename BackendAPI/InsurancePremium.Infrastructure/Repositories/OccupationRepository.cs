using InsurancePremium.Core.Entities;
using InsurancePremium.Core.Interfaces;

namespace InsurancePremium.Infrastructure.Repositories
{
   
    public class OccupationRepository : IOccupationRepository
    {
        private static readonly List<OccupationRating> _ratings = new()
        {
            new OccupationRating { Id = 1, RatingName = "Professional", Factor = 1.50m },
            new OccupationRating { Id = 2, RatingName = "White Collar",  Factor = 2.25m },
            new OccupationRating { Id = 3, RatingName = "Light Manual",  Factor = 11.50m },
            new OccupationRating { Id = 4, RatingName = "Heavy Manual",  Factor = 31.75m }
        };

        private static readonly List<Occupation> _occupations = new()
        {
            new Occupation { Id = 1, Name = "Cleaner",  Rating = "Light Manual" },
            new Occupation { Id = 2, Name = "Doctor",   Rating = "Professional" },
            new Occupation { Id = 3, Name = "Author",   Rating = "White Collar" },
            new Occupation { Id = 4, Name = "Farmer",   Rating = "Heavy Manual" },
            new Occupation { Id = 5, Name = "Mechanic", Rating = "Heavy Manual" },
            new Occupation { Id = 6, Name = "Florist",  Rating = "Light Manual" },
            new Occupation { Id = 7, Name = "Other",    Rating = "Heavy Manual" }
        };

        public Task<IEnumerable<Occupation>> GetAllOccupationsAsync()
            => Task.FromResult<IEnumerable<Occupation>>(_occupations);
        public Task<Occupation?> GetOccupationByIdAsync(int id)
            => Task.FromResult(_occupations.FirstOrDefault(o => o.Id == id));
        public Task<IEnumerable<OccupationRating>> GetAllRatingsAsync()
            => Task.FromResult<IEnumerable<OccupationRating>>(_ratings);
        public Task<OccupationRating?> GetRatingByNameAsync(string ratingName)
            => Task.FromResult(
                _ratings.FirstOrDefault(r =>
                    r.RatingName.Equals(ratingName, StringComparison.OrdinalIgnoreCase)));
    }
}

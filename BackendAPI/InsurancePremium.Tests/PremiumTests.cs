using InsurancePremium.Core.Entities;
using InsurancePremium.Core.Interfaces;
using InsurancePremium.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace InsurancePremium.Tests
{
    public class PremiumCalculationServiceTests
    {
        private readonly Mock<IOccupationRepository> _repositoryMock;
        private readonly Mock<ILogger<PremiumCalculationService>> _loggerMock;
        private readonly PremiumCalculationService _service;

        public PremiumCalculationServiceTests()
        {
            _repositoryMock = new Mock<IOccupationRepository>();
            _loggerMock     = new Mock<ILogger<PremiumCalculationService>>();
            _service        = new PremiumCalculationService(_repositoryMock.Object, _loggerMock.Object);
        }


        [Fact]
        [Trait("Category", "PremiumCalculation")]
        public async Task CalculateMonthlyPremium_Doctor_ReturnsCorrectPremium()
        {
            _repositoryMock
                .Setup(r => r.GetOccupationByIdAsync(2))
                .ReturnsAsync(new Occupation { Id = 2, Name = "Doctor", Rating = "Professional" });

            _repositoryMock
                .Setup(r => r.GetRatingByNameAsync("Professional"))
                .ReturnsAsync(new OccupationRating { Id = 1, RatingName = "Professional", Factor = 1.5m });

            var result = await _service.CalculateMonthlyPremiumAsync(500_000m, 2, 30);

            Assert.Equal(1875.00m, result);
        }

        [Fact]
        [Trait("Category", "PremiumCalculation")]
        public async Task CalculateMonthlyPremium_Farmer_ReturnsCorrectPremium()
        {
            _repositoryMock
                .Setup(r => r.GetOccupationByIdAsync(4))
                .ReturnsAsync(new Occupation { Id = 4, Name = "Farmer", Rating = "Heavy Manual" });

            _repositoryMock
                .Setup(r => r.GetRatingByNameAsync("Heavy Manual"))
                .ReturnsAsync(new OccupationRating { Id = 4, RatingName = "Heavy Manual", Factor = 31.75m });

            var result = await _service.CalculateMonthlyPremiumAsync(200_000m, 4, 25);

            Assert.Equal(13229.17m, result);
        }

        [Fact]
        [Trait("Category", "PremiumCalculation")]
        public async Task CalculateMonthlyPremium_InvalidOccupation_ThrowsArgumentException()
        {
            _repositoryMock
                .Setup(r => r.GetOccupationByIdAsync(999))
                .ReturnsAsync((Occupation?)null);

            await Assert.ThrowsAsync<ArgumentException>(
                () => _service.CalculateMonthlyPremiumAsync(100_000m, 999, 35));
        }

        [Fact]
        [Trait("Category", "PremiumCalculation")]
        public async Task CalculateMonthlyPremium_ZeroDeathCover_ThrowsArgumentOutOfRangeException()
        {
            _repositoryMock
                .Setup(r => r.GetOccupationByIdAsync(1))
                .ReturnsAsync(new Occupation { Id = 1, Name = "Cleaner", Rating = "Light Manual" });

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                () => _service.CalculateMonthlyPremiumAsync(0m, 1, 30));
        }

        [Fact]
        [Trait("Category", "PremiumCalculation")]
        public async Task CalculateMonthlyPremium_NegativeAge_ThrowsArgumentOutOfRangeException()
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                () => _service.CalculateMonthlyPremiumAsync(100_000m, 1, -1));
        }

        [Fact]
        [Trait("Category", "PremiumCalculation")]
        public async Task CalculateMonthlyPremium_Author_WhiteCollar_ReturnsCorrectPremium()
        {
            _repositoryMock
                .Setup(r => r.GetOccupationByIdAsync(3))
                .ReturnsAsync(new Occupation { Id = 3, Name = "Author", Rating = "White Collar" });

            _repositoryMock
                .Setup(r => r.GetRatingByNameAsync("White Collar"))
                .ReturnsAsync(new OccupationRating { Id = 2, RatingName = "White Collar", Factor = 2.25m });

            var result = await _service.CalculateMonthlyPremiumAsync(300_000m, 3, 40);

            Assert.Equal(2250.00m, result);
        }
    }

    public class OccupationRepositoryTests
    {
        private readonly InsurancePremium.Infrastructure.Repositories.OccupationRepository _repository;

        public OccupationRepositoryTests()
        {
            _repository = new InsurancePremium.Infrastructure.Repositories.OccupationRepository();
        }

        [Fact]
        public async Task GetAllOccupations_Returns7Occupations()
        {
            var result = await _repository.GetAllOccupationsAsync();
            Assert.Equal(7, result.Count());
        }

        [Fact]
        public async Task GetOccupationById_ValidId_ReturnsOccupation()
        {
            var result = await _repository.GetOccupationByIdAsync(1);
            Assert.NotNull(result);
            Assert.Equal("Cleaner", result!.Name);
            Assert.Equal("Light Manual", result.Rating);
        }

        [Fact]
        public async Task GetOccupationById_InvalidId_ReturnsNull()
        {
            var result = await _repository.GetOccupationByIdAsync(999);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllRatings_Returns4Ratings()
        {
            var result = await _repository.GetAllRatingsAsync();
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public async Task GetRatingByName_Professional_ReturnsFactor1Point5()
        {
            var result = await _repository.GetRatingByNameAsync("Professional");
            Assert.NotNull(result);
            Assert.Equal(1.5m, result!.Factor);
        }

        [Fact]
        public async Task GetRatingByName_HeavyManual_ReturnsFactor31Point75()
        {
            var result = await _repository.GetRatingByNameAsync("Heavy Manual");
            Assert.NotNull(result);
            Assert.Equal(31.75m, result!.Factor);
        }

        [Fact]
        public async Task GetRatingByName_NonExistent_ReturnsNull()
        {
            var result = await _repository.GetRatingByNameAsync("Unknown Rating");
            Assert.Null(result);
        }
    }
}

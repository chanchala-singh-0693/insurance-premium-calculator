using InsurancePremium.API.Models;
using InsurancePremium.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InsurancePremium.API.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PremiumCalculationController : ControllerBase
    {
        private readonly IPremiumCalculationService _premiumService;
        private readonly IOccupationRepository _occupationRepository;
        private readonly ILogger<PremiumCalculationController> _logger;

        
        public PremiumCalculationController(
            IPremiumCalculationService premiumService,
            IOccupationRepository occupationRepository,
            ILogger<PremiumCalculationController> logger)
        {
            _premiumService = premiumService;
            _occupationRepository = occupationRepository;
            _logger = logger;
        }

        
        [HttpPost("calculate")]
        [ProducesResponseType(typeof(PremiumResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CalculatePremium([FromBody] PremiumRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request model: {Errors}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation(
                    "Premium calculation request received for member '{Name}', OccupationId={Id}",
                    request.Name, request.OccupationId);

                var occupation = await _occupationRepository.GetOccupationByIdAsync(request.OccupationId);
                if (occupation == null)
                {
                    _logger.LogWarning("Occupation not found for Id={Id}", request.OccupationId);
                    return NotFound(new { message = $"Occupation with ID {request.OccupationId} was not found." });
                }

                var factor = await _premiumService.GetOccupationFactorAsync(request.OccupationId);

                var monthlyPremium = await _premiumService.CalculateMonthlyPremiumAsync(
                    request.DeathSumInsured,
                    request.OccupationId,
                    request.AgeNextBirthday);

                var response = new PremiumResponse
                {
                    MemberName        = request.Name,
                    OccupationName    = occupation.Name,
                    OccupationRating  = occupation.Rating,
                    OccupationFactor  = factor,
                    MonthlyPremium    = monthlyPremium,
                    CalculatedAt      = DateTime.UtcNow
                };

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Business validation error during premium calculation.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during premium calculation.");
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again." });
            }
        }
    }
}

using InsurancePremium.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InsurancePremium.API.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class OccupationController : ControllerBase
    {
        private readonly IOccupationRepository _occupationRepository;
        private readonly ILogger<OccupationController> _logger;

        public OccupationController(
            IOccupationRepository occupationRepository,
            ILogger<OccupationController> logger)
        {
            _occupationRepository = occupationRepository;
            _logger = logger;
        }

        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOccupations()
        {
            try
            {
                _logger.LogInformation("Fetching all occupations.");
                var occupations = await _occupationRepository.GetAllOccupationsAsync();
                return Ok(occupations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching occupations.");
                return StatusCode(500, new { message = "An error occurred while retrieving occupations." });
            }
        }

        
        [HttpGet("ratings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRatings()
        {
            try
            {
                _logger.LogInformation("Fetching all occupation ratings.");
                var ratings = await _occupationRepository.GetAllRatingsAsync();
                return Ok(ratings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching ratings.");
                return StatusCode(500, new { message = "An error occurred while retrieving ratings." });
            }
        }
    }
}

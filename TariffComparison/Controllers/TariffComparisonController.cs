using Microsoft.AspNetCore.Mvc;
using TariffComparison.Repository;

namespace TariffComparison.Controllers
{
    [Controller]
    [Route("/api/[controller]")]
    public class TariffComparisonController : ControllerBase
    {
        private readonly ITariffComparisonRepository _repository;
        private readonly ILogger<TariffComparisonController> _logger;

        public TariffComparisonController(ITariffComparisonRepository repository,ILogger<TariffComparisonController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Get method to compare tariffs
        /// </summary>
        /// <param name="consumption"></param>
        /// <returns></returns>
        [HttpGet("compare")]
        public async Task<IActionResult> CompareTariffs([FromQuery] int consumption)
        {
            if (consumption <= 0)
                return BadRequest("Consumption must be greater than 0.");

            try
            {
                var results = await _repository.CompareTariffsAsync(consumption);
                if(results==null || !results.Any())
                    return StatusCode(StatusCodes.Status500InternalServerError
                        ,"Error occured while processing request.");

                return Ok(results);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex,"Unknown error occured.");
                return StatusCode(StatusCodes.Status500InternalServerError
                        , $"Error occured while processing request.");
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using WeatherApp_API.Models;
using WeatherApp_API.Services;

namespace WeatherApp_API.Controllers
{
    [Route("api/WeatherAPI")]
    [ApiController]
    public class WeatherAPIController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        //added to log information for debugging purposes
        private readonly ILogger<WeatherAPIController> _logger;
        public WeatherAPIController(IWeatherService weatherService, ILogger<WeatherAPIController> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }
        [HttpGet(Name = "GetWeathers")]
        //documenting status codes
        [ProducesResponseType(StatusCodes.Status200OK)] //case of success
        [ProducesResponseType(StatusCodes.Status404NotFound)] //if not found
        public async Task<ActionResult<IEnumerable<Weather>>> GetAllWeathers()
        {
            try
            {
                _logger.LogInformation("Attempting to get all weathers...");
                var weathers = await _weatherService.GetAllWeathersAsync();
                _logger.LogInformation("Successfully retrieved weathers.");
                return weathers.Any() ? Ok(weathers) : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching weather data.");
                return StatusCode(500, "An error occured while fetching weather data.");
            }

        }
        [HttpGet("{Id:int}", Name = "GetWeather")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Weather>> GetWeatherById(int Id)
        {
            try
            {
                _logger.LogInformation("Attempting to get weather by id...");
                var weather = await _weatherService.GetWeatherByIdAsync(Id);
                _logger.LogInformation("Successfully retrieved weather data.");
                return weather != null ? Ok(weather) : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching weather data.");
                return BadRequest();
            }
        }
        [HttpPost(Name = "AddWeather")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddWeather(Weather weather)
        {
            try
            {
                _logger.LogInformation("Attempting to add weather data...");
                await _weatherService.AddWeatherAsync(weather);
                _logger.LogInformation("Successfully added weather data.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching weather data.");
                return BadRequest();
            }
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id:int}", Name = "UpdateWeather")]
        public async Task<ActionResult> UpdateWeather(int id, Weather weather)
        {
            try
            {
                var existingWeather = await _weatherService.GetWeatherByIdAsync(id);
                if (existingWeather == null)
                {
                    return NotFound($"Weather with ID {id} not found");
                }
                _logger.LogInformation("Attempting to update weather data...");
                await _weatherService.UpdateWeatherAsync(weather);
                _logger.LogInformation("Successfully updated weather data.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating weather data.");
                return StatusCode(500, "An error occurred while updating weather data.");
            }
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{Id:int}", Name = "DeleteWeather")]
        public async Task<ActionResult> DeleteWeather(int Id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete weather data...");
                await _weatherService.DeleteWeatherAsync(Id);
                _logger.LogInformation("Successfully deleted weather data.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting weather data.");
                return StatusCode(500, "An error occured while deleting weather data.");
            }
        }
    }
}
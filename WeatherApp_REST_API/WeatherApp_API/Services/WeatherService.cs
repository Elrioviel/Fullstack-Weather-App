using WeatherApp_API.Models;
using WeatherApp_API.Repositories;

namespace WeatherApp_API.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherRepository _weatherRepository;
        public WeatherService(IWeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository ?? throw new ArgumentNullException(nameof(weatherRepository));
        }
        public async Task AddWeatherAsync(Weather weather)
        {
            try
            {
                await _weatherRepository.AddWeatherAsync(weather);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task UpdateWeatherAsync(Weather weather)
        {
            try
            {
                await _weatherRepository.UpdateWeatherAsync(weather);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task DeleteWeatherAsync(int id)
        {
            try
            {
                await _weatherRepository.DeleteWeatherAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<Weather>> GetAllWeathersAsync()
        {
            try
            {
                var weathers = await _weatherRepository.GetAllWeathersAsync();
                return weathers;
            }
            catch
            {
                return null;
            }
        }
        public async Task<Weather> GetWeatherByIdAsync(int id)
        {
            try
            {
                var weather = await _weatherRepository.GetWeatherByIdAsync(id);
                return weather;
            }
            catch
            {
                return null;
            }
        }
    }
}
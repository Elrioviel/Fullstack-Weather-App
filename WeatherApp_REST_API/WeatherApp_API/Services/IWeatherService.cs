using WeatherApp_API.Models;

namespace WeatherApp_API.Services
{
    public interface IWeatherService
    {
        Task<IEnumerable<Weather>> GetAllWeathersAsync();
        Task<Weather> GetWeatherByIdAsync(int id);
        Task AddWeatherAsync(Weather weather);
        Task UpdateWeatherAsync(Weather weather);
        Task DeleteWeatherAsync(int id);
    }
}
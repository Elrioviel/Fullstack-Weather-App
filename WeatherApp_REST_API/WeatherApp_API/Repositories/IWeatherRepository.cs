using WeatherApp_API.Models;

namespace WeatherApp_API.Repositories
{
    public interface IWeatherRepository
    {
        Task<IEnumerable<Weather>> GetAllWeathersAsync();
        Task<Weather> GetWeatherByIdAsync(int id);
        Task AddWeatherAsync(Weather weather);
        Task UpdateWeatherAsync(Weather weather);
        Task DeleteWeatherAsync(int id);
    }
}
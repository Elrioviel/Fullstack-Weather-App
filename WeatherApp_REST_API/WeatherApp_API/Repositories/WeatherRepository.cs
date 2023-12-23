using Dapper;
using System.Data.SqlClient;
using WeatherApp_API.Models;

namespace WeatherApp_API.Repositories
{
    internal class WeatherRepository : IWeatherRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public WeatherRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _connectionString = _configuration.GetConnectionString("WeatherAppCon");

        }
        //helper method for opening a connection and executing a query
        private async Task<SqlConnection> OpenConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
        public async Task AddWeatherAsync(Weather weather)
        {
            const string query = @"INSERT INTO dbo.Weathers (Date, Country, City, Condition, Temperature) VALUES (@Date, @Country, @City, @Condition, @Temperature)";
            using (SqlConnection myCon = await OpenConnectionAsync())
            {
                await myCon.ExecuteAsync(query, weather);
            }
        }
        public async Task DeleteWeatherAsync(int id)
        {
            const string query = @"DELETE FROM dbo.Weathers WHERE WeatherID=@WeatherID";
            using (SqlConnection myCon = await OpenConnectionAsync())
            {
                await myCon.ExecuteAsync(query, new { WeatherID = id });
            }
        }
        public async Task<Weather> GetWeatherByIdAsync(int id)
        {
            const string query = @"SELECT * FROM dbo.Weathers WHERE WeatherID = @id";
            using (SqlConnection myCon = await OpenConnectionAsync())
            {
                return await myCon.QueryFirstOrDefaultAsync<Weather>(query, new { id });
            }
        }
        public async Task UpdateWeatherAsync(Weather weather)
        {
            const string query = @"UPDATE dbo.Weathers SET Date=@Date, Country=@Country, City=@City, Condition=@Condition, Temperature=@Temperature WHERE WeatherID=@WeatherID";

            using (SqlConnection myCon = await OpenConnectionAsync())
            {
                await myCon.ExecuteAsync(query, new
                {
                    weather.WeatherID,
                    weather.Date,
                    weather.Country,
                    weather.City,
                    weather.Condition,
                    weather.Temperature
                });
            }
        }
        public async Task<IEnumerable<Weather>> GetAllWeathersAsync()
        {
            const string query = @"SELECT * FROM dbo.Weathers";
            using (SqlConnection myCon = await OpenConnectionAsync())
            {
                return await myCon.QueryAsync<Weather>(query);
            }
        }
    }
}
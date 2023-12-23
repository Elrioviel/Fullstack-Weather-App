using System.ComponentModel.DataAnnotations;

namespace WeatherApp_API.Models
{
    public class Weather
    {
        public int? WeatherID { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        [MaxLength(50)]
        public string Country { get; set; }
        [Required]
        [MaxLength(50)]
        public string City { get; set; }
        [Required]
        [MaxLength(50)]
        public string Condition { get; set; }
        [Required]
        public int Temperature { get; set; }
    }
}
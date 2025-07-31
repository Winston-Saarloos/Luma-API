using Luma_API.Models;

namespace Luma_API.Services
{
    public interface IWeatherService
    {
        Task<WeatherDto>GetWeatherAsync(double lat, double lng);
    }
}

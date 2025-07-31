namespace Luma_API.Services
{
    public interface IGeocodingService
    {
        Task<(double lat, double lng)> GetCoordinatesAsync(string city);
    }
}

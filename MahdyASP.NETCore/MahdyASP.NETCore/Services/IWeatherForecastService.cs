namespace MahdyASP.NETCore.Services
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> GetForecastes();
    }
}

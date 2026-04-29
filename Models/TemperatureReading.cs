namespace temperature_system.Models;

public class TemperatureReading
{
    public int Id { get; set; }

    public required string DeviceId { get; set; }

    public double TemperatureCelsius { get; set; }

    public double? Humidity { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
}
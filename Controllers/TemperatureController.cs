using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using temperature_system.Data;
using temperature_system.Models;

namespace temperature_system.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemperatureController(AppDbContext db, ILogger<TemperatureController> logger) : ControllerBase
{
    private readonly AppDbContext _db = db;
    private readonly ILogger<TemperatureController> _logger = logger;

    [HttpPost]
    [ProducesResponseType(typeof(TemperatureReading), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostReading([FromBody] TemperatureReadingDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var reading = new TemperatureReading
        {
            DeviceId = dto.DeviceId,
            TemperatureCelsius = dto.TemperatureCelsius,
            Humidity = dto.Humidity,

            Timestamp = dto.Timestamp ?? DateTime.UtcNow,
            ReceivedAt = DateTime.UtcNow
        };

        _db.TemperatureReadings.Add(reading);
        await _db.SaveChangesAsync();

        _logger.LogInformation(
            "Citire noua primita de la {DeviceId}: {Temp}C la {Time}",
            reading.DeviceId, reading.TemperatureCelsius, reading.Timestamp);

        return CreatedAtAction(nameof(GetReading), new { id = reading.Id }, reading);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TemperatureReading), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReading(int id)
    {
        var reading = await _db.TemperatureReadings.FindAsync(id);
        return reading is null ? NotFound() : Ok(reading);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TemperatureReading>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReadings(
        [FromQuery] string? deviceId,
        [FromQuery] int limit = 50)
    {
        var query = _db.TemperatureReadings.AsQueryable();

        if (!string.IsNullOrWhiteSpace(deviceId))
            query = query.Where(r => r.DeviceId == deviceId);

        var results = await query
            .OrderByDescending(r => r.Timestamp)
            .Take(limit)
            .ToListAsync();

        return Ok(results);
    }
}

public class TemperatureReadingDto
{
    [System.ComponentModel.DataAnnotations.Required]
    public required string DeviceId { get; set; }

    public double TemperatureCelsius { get; set; }

    public double? Humidity { get; set; }

    public DateTime? Timestamp { get; set; }
}
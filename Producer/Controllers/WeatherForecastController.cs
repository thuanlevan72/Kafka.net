using Consumer.Models;
using Microsoft.AspNetCore.Mvc;
using Producer.Services;

namespace Producer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly KafkaProducer _producer;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _producer = new KafkaProducer();
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromQuery] string message)
        {
            await _producer.SendMessageAsync(message);
            return Ok("Message sent to Kafka!");
        }
         
        [HttpPost]
        public async Task<IActionResult> CreateTodo([FromBody] Todo todo)
        {
            await _producer.SendMessageAsync(todo);
            return Ok(new { Message = "Todo Created", Todo = todo });
        }
    }
}

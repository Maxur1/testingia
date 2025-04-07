using Microsoft.AspNetCore.Mvc;
using testingia.Services;

namespace testingia.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PredictController : ControllerBase
    {
        private readonly PredictionService _predictionService;

        public PredictController()
        {
            _predictionService = new PredictionService();
        }

        [HttpGet("{productName}")]
        public IActionResult PredictSegment(string productName)
        {
            var result = _predictionService.Predict(productName);
            return Ok(new { ProductName = productName, PredictedClass = result });
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using tweet_model_ml_api.Services.TweetService;

namespace tweet_model_ml_api.Controllers
{
    [ApiController]
    [Route("[controller]")]    
    public class TweetController : ControllerBase
    {
        private readonly ILogger<TweetController> _logger;
        private readonly ITweetService _tweetService;
       public TweetController(ILogger<TweetController> logger, ITweetService tweetService) 
        {
            _logger = logger;
            _tweetService = tweetService;
        }

        [HttpGet("{message}")]
        public async Task<IActionResult> MessageAsync(string message)
        {
            try
            {
                var retVal = await _tweetService.MessageAsync(message);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            
        }
    }
}

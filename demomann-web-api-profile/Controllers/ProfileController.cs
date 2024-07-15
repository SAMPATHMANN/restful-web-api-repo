using System;
using System.Linq.Expressions;
using System.Net;
using DemomannWebApi.Profile.Services;
using Microsoft.AspNetCore.Mvc;
using ServiceModels = DemomannWebApi.Profile.Services.Models;

namespace DemomannWebApi.Profile.Controllers
{
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly IProfileService _profileService;
        public ProfileController(ILogger<ProfileController> logger, IProfileService profileService ){
            _logger =logger;
            _profileService = profileService;
        }

        [HttpGet("getlist")]
        public async Task<IActionResult> GetProfilesAsync()
        {
            try {
                var retVal = await _profileService.GetProfilesAsync();
                return Ok(retVal);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            
        }

        [HttpPost("save")]
        public async Task<IActionResult> PostProfile(ServiceModels.Profile request){
            try {
                var retVal = await _profileService.SaveProfileAsync(request);
                return Ok(retVal);
            }
            catch (Exception ex){
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }



    }
}
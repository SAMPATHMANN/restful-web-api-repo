using System.Net;
using Demomann.common.enums;
using DemomannWebApi.Profile.Services;
using DemomannWebApi.Profile.Services.Models;
using Microsoft.AspNetCore.Mvc;
using ServiceModels = DemomannWebApi.Profile.Services.Models;

namespace DemomannWebApi.Profile.Controllers
{
    [ApiController]
    [Route("[controller]")]
    // [EnableCors()]
    public class ProfileController : ControllerBase
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly IProfileService _profileService;
        public ProfileController(ILogger<ProfileController> logger, IProfileService profileService ){
            _logger =logger;
            _profileService = profileService;
        }

        [HttpGet("getactiontypes")]
        public async Task<IActionResult> GetProfileActionTypes(){
            try{
                var retVal = await _profileService.GetProfileActionTypesAsync();
                return Ok(retVal);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("get")]
        [ProducesResponseType<ProfileSearch>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProfilesAsync([FromQuery] ProfileSearch? profileSearch = null )
        {
            try {
                var retVal = await _profileService.GetProfilesAsync(profileSearch);
                return Ok(retVal);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            
        }

        [HttpPost("save/{saveType}")]
        public async Task<IActionResult> PostProfile(ProfileActionTypeEnum saveType, ServiceModels.Profile request){
            try {
                var retVal = await _profileService.SaveProfileAsync(saveType, request);
                return Ok(retVal);
            }
            catch (Exception ex){
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }



    }
}
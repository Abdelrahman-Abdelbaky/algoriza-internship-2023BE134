
using Humanizer.Localisation;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using VezeetaProject.Core.Dtos.AppointmentDtos;
using VezeetaProject.Core.Resources;

namespace VezeetaProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Doctor")]
    public class AppointmenController : ControllerBase
    {
        private readonly IAppointmentServices _appointmentServices;
        private readonly IStringLocalizer<SharedResources> _localizer;
   

        public AppointmenController(IAppointmentServices appointmentServices, IStringLocalizer<SharedResources> localizer)
        {
            _appointmentServices = appointmentServices;
            _localizer = localizer;
        }

        [HttpPost("Appointment/Add")]
        public async Task<IActionResult> Add( [FromBody]AddAppointmentDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var UserId = HttpContext.User.FindFirstValue("uid");
      
            foreach (var item in model.Appointment)
            {

                if ((int)item.day < 0 || (int)item.day > 6)
                {
                    return BadRequest(_localizer[ResourceItem.DayErrorInput].ToString());
                }
            }


            if (UserId == null)
            {
                return Unauthorized();
            }

            
            var result = await _appointmentServices.AddAppointmentAsync (model, UserId);

            if (!result.IsDone ) { return NotFound(_localizer[ResourceItem.NotFound]); }
            if (result.ErrorMassage is not null ) { return Ok(result.ErrorMassage); }

            return Ok(_localizer[ResourceItem.AddSuccessfully].ToString());
        }

        [HttpPut("Appointment/Update")]
        public async Task<IActionResult> Update([FromBody] ModfiyAppointmentDto model) {

            var UserId = HttpContext.User.FindFirstValue("uid");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

          

            if (UserId == null)
            {
                return Unauthorized();
            }

            var result = await _appointmentServices.UpdateAppointmentAsync(model, UserId);

            if (result.ErrorMassage is not null) { return NotFound(result.ErrorMassage); }

            if (!result.IsDone) { return BadRequest(_localizer[ResourceItem.UpdateFailed].ToString());}

            return Ok(_localizer[ResourceItem.UpdateSuccessfully].ToString());
        }

        [HttpDelete("Appointment/Delete")]
        public async Task<IActionResult> Delete([FromBody] int id) {

            var UserId = HttpContext.User.FindFirstValue("uid");

            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
        
            
            if (UserId == null)
            {
                return Unauthorized();
            }

           
            var result = await _appointmentServices.DeleteAppointmentAsync(id, UserId);

            if (result.ErrorMassage is not null) { return NotFound(result.ErrorMassage); }
            if (!result.IsDone) { return BadRequest(_localizer[ResourceItem.DeleteFailed].ToString()); }

            return Ok(_localizer[ResourceItem.DeleteSuceesfully].ToString());
        }
    }
}


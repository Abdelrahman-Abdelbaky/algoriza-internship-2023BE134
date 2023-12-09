using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using VezeetaProject.Core.Dtos;
using VezeetaProject.Core.Dtos.Booking;
using VezeetaProject.Core.Resources;

namespace VezeetaProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public BookingController(IBookingService bookingService ,IStringLocalizer<SharedResources> localizer )
        {
            _bookingService = bookingService;
            _localizer = localizer;
        }

        [HttpPost("Booking")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Booking([FromBody] AddBookingDto model) { 
        
          if(!ModelState.IsValid)
                return BadRequest(ModelState);
          if (model.TimeId <= 0) 
                return BadRequest(_localizer[ResourceItem.theValueCanNotBeEqualOrLessThanZero]);
            var UserId = HttpContext.User.FindFirstValue("uid");
            var result = await _bookingService.Booking(model, UserId);

            if (result.ErrorMassage != null) { 
                return BadRequest(result.ErrorMassage);
            }

            return Ok(_localizer[ResourceItem.AddSuccessfully]);
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetAll ()
        {

            var UserId = HttpContext.User.FindFirstValue("uid");

            var result = await _bookingService.GetAll(UserId);

            if (result.Count() is 0) { return NotFound( _localizer[ResourceItem.NotFound]); }
         
            return Ok(result);

        }

        [HttpGet("GetAllBookingForDoctor")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetAllBookingPatientForDoctor([FromQuery]SearchDto2 model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.page <= 0)
                return BadRequest(_localizer[ResourceItem.numberOfPagenotLessThan1].ToString());

            if (model.pageSize <= 0)
                return BadRequest(_localizer[ResourceItem.numberOfPagenotLessThan1].ToString());
            
            var UserId = HttpContext.User.FindFirstValue("uid");

            var result = await _bookingService.GetAllBookingAppointmentForDoctor(model.page, model.pageSize, model.Day, UserId);
            if (model is null )
                return NotFound(_localizer[ResourceItem.NotFound].ToString());

            return Ok(result);

        }


        [HttpPut("ConfirmCheckUp")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> ConfirmCheckUp ([FromBody] int DoctorId)
        {
            if (DoctorId <= 0)
            {
                return BadRequest(_localizer[ResourceItem.theValueCanNotBeEqualOrLessThanZero]);
            }
            var UserId = HttpContext.User.FindFirstValue("uid");

            var result = await _bookingService.ConfiromCheckUp(DoctorId, UserId);

            if (result.ErrorMassage is not null) { return NotFound(result.ErrorMassage); }

            return Ok(_localizer[ResourceItem.UpdateSuccessfully]);
        }

        [HttpPut("Canceled")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Canceled([FromBody] int Id)
        {
            if (Id <= 0) {
                return BadRequest(_localizer[ResourceItem.theValueCanNotBeEqualOrLessThanZero]);
            }

            var result = await _bookingService.CancelBooking(Id);

            if (result.ErrorMassage is not null) { return NotFound(result.ErrorMassage); }

            return Ok(_localizer[ResourceItem.UpdateSuccessfully]);

        }


    }
}

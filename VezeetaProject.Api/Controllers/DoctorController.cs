using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VezeetaProject.Core.Resources;

namespace VezeetaProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoctorController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        private readonly IDoctorService _doctorService;
        private readonly IImageService _imageService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public DoctorController(IDiscountService discountService, IDoctorService doctorService , IStringLocalizer<SharedResources> Localizer, IImageService imageService)
        {
            _discountService = discountService;
            _doctorService = doctorService;
            _localizer = Localizer;
            _imageService = imageService;
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin , Patient")]
        public async Task<IActionResult> GetAll([FromQuery] SearchDto searchDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (searchDto.page <= 0)
                return BadRequest(_localizer[ResourceItem.numberOfPagenotLessThan1].ToString());

            if (searchDto.pageSize <= 0)
                return BadRequest(_localizer[ResourceItem.numberOfPagenotLessThan1].ToString());

          
            if (HttpContext.User.IsInRole("Admin"))
            {
                var result = await _doctorService.GetAllDoctor(searchDto.page, searchDto.pageSize, searchDto.search);
                if (result is null || result.Count == 0)
                    return NotFound(_localizer[ResourceItem.NotFound].ToString());
                return Ok(result);
            }
            if (HttpContext.User.IsInRole("Patient"))
            {
                var result = await _doctorService.GetAllDoctorWithAppointment(searchDto.page, searchDto.pageSize, searchDto.search); ;
                
                if (result is null || result.Count == 0)
                    return NotFound(_localizer[ResourceItem.NotFound].ToString());
                 else return Ok(result);
            }
           
             return NotFound(_localizer[ResourceItem.NotFound].ToString());
        }

        [HttpGet("GetById")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDoctorById([FromQuery] int Id)
        {
            if (Id <= 0) return BadRequest(_localizer[ResourceItem.theIdMustBeNotLessthan1].ToString());

            var result = await _doctorService.GetDoctorById(Id);

            if (!result.IsDone) return NotFound(result.ErrorMassage);

            return Ok(result.Object);
        }


        [HttpPost("Add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDoctor([FromForm] DoctorDto model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_imageService.CheckTypeOfImage(model.Image))
                return BadRequest(_localizer[ResourceItem.ImageTypeError].ToString());

            var ResultDto = await _doctorService.AddDoctor(model);

            if (!ResultDto.IsDone)
                return BadRequest(ResultDto.ErrorMassage);

            return Ok(_localizer[ResourceItem.AddSuccessfully].ToString());
        }

        [HttpPut("Update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDoctor([FromForm] UpdateDoctorDto model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_imageService.CheckTypeOfImage(model.Image))
                return BadRequest(_localizer[ResourceItem.ImageTypeError].ToString());
            

            var resultDto = await _doctorService.UpdateDoctor(model);

            if (!resultDto.IsDone)
                return NotFound(resultDto.ErrorMassage);

            return Ok(_localizer[ResourceItem.UpdateSuccessfully].ToString());
        }

        [HttpDelete("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDoctor([FromBody] int Id)
        {

            if (Id <= 0) return BadRequest(_localizer[ResourceItem.theIdMustBeNotLessthan1].ToString());

            var result = await _doctorService.DeleteDoctor(Id);

            if (!result.IsDone) return NotFound(result.ErrorMassage);

            return Ok(_localizer[ResourceItem.DeleteSuceesfully].ToString());
        }
    }
}

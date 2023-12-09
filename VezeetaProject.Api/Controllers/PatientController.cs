using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VezeetaProject.Core.Resources;

namespace VezeetaProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IpatientService _patientService;

        public PatientController(IStringLocalizer<SharedResources> localizer, IpatientService patientService)
        {
            _localizer = localizer;
            _patientService = patientService;
        }
     
        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] SearchDto searchDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (searchDto.page <= 0)
                return BadRequest(_localizer[ResourceItem.numberOfPagenotLessThan1].ToString());

            if (searchDto.pageSize <= 0)
                return BadRequest(_localizer[ResourceItem.numberOfPagenotLessThan1].ToString());
            
            var result = await _patientService.GeTAllPatient(searchDto.page, searchDto.pageSize, searchDto.search);

            if (result is null || result.Object.Count == 0)
                return NotFound(_localizer[ResourceItem.NotFound].ToString());

            return Ok(result.Object);
        }

        [HttpGet("GetById")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {

            if (id is  null)
                return BadRequest();

            var result = await _patientService.GeTBYId(id);

            if (result is null)
                return NotFound(_localizer[ResourceItem.NotFound].ToString());


            return Ok(result);
        }
    
}
}

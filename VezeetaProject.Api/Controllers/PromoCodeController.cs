using Microsoft.Extensions.Localization;
using VezeetaProject.Core.Resources;

namespace VezeetaProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromoCodeController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        private readonly  IStringLocalizer<SharedResources> _localizer;
      
        public PromoCodeController(IDiscountService discountService , IStringLocalizer<SharedResources> Localizer)
        {
            _discountService = discountService;
            _localizer = Localizer;
        }     

        [HttpPost("Add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDicount([FromBody] DiscountDto model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Value < 0)
                return BadRequest(_localizer[ResourceItem.TheValueCannotBeLessThan0].ToString());

            if (model.DiscountType == DiscountType.Percentage && model.Value > 100)
                return BadRequest(_localizer[ResourceItem.TheValueCannotBeGreaterThan100].ToString());

            var result = await _discountService.AddDiscountAsync(model);

            if (result == null)
                return BadRequest(_localizer[ResourceItem.DiscountCode].ToString());

            return Ok(result);
        }

        [HttpDelete("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDiscount([FromBody] int Id)
        {

            var result = await _discountService.DeleteDiscountAsync(Id);
            if (result is null) return NotFound(_localizer[ResourceItem.NotFound].ToString());
            return Ok(_localizer[ResourceItem.DeleteSuceesfully].ToString());
        }
        
        [HttpPut("Update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDiscount([FromBody] UpdateDiscountDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if (model.Value <= 0)
                return BadRequest(_localizer[ResourceItem.theValueCanNotBeEqualOrLessThanZero].ToString());
            
            if (model.requsetCompleted < 0)
                return BadRequest(_localizer[ResourceItem.TheValueCannotBeLessThan0].ToString());

            var result = await _discountService.UpdateDiscountAsync(model);

            if (result is null)
                return BadRequest(_localizer[ResourceItem.DiscountError].ToString());

            return Ok(_localizer[ResourceItem.UpdateSuccessfully].ToString());
        }
     
        [HttpPut("Deactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeactivateDiscount([FromBody] int Id)
        {
            var result = await _discountService.DeactivateDiscountAsync(Id);
            if (result is null) return NotFound(_localizer[ResourceItem.NotFound].ToString());
            return Ok(_localizer[ResourceItem.UpdateSuccessfully].ToString());
        }

    }
}

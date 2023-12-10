using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using SearchFilter = VezeetaProject.Core.Consts.SearchFilter;

namespace VezeetaProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticsServes _statisticsServes;

        public StatisticController(IStatisticsServes statisticsServes)
        {
            _statisticsServes = statisticsServes;
        }


        [HttpGet("NumOfDoctors")]
        public async Task<IActionResult> NumOfDoctors(SearchFilter filter) {

          if ((int)filter < 0 && (int)filter > 4) return BadRequest();

          return Ok(await _statisticsServes.NumOfDoctors(filter));
        }


        [HttpGet("NumOfPatients")]
        public async Task<IActionResult> NumOfPatients(SearchFilter filter)
        {

            if ((int)filter < 0 && (int)filter > 4) return BadRequest();

            return Ok(await _statisticsServes.NumOfPatients(filter));
        }


        [HttpGet("NumOfRequests")]
        public async Task<IActionResult> NumOfRequests(SearchFilter filter)
        {

            if ((int)filter < 0 && (int)filter > 4) return BadRequest();

            return Ok(await _statisticsServes.NumOfRequests(filter));
        }
        [HttpGet("Top5Specializations")]
        public async Task<IActionResult> Top5Specializations()
        {
            return Ok(await _statisticsServes.TopSpecializations(5));
        }
        [HttpGet("Top10Doctors")]
        public async Task<IActionResult> Top10Doctors()
        {
            return Ok(await _statisticsServes.TopDoctors(10));
        }
    }
}

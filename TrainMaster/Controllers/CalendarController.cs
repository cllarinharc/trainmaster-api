using Microsoft.AspNetCore.Mvc;
using TrainMaster.Application.UnitOfWork;
using TrainMaster.Domain.Dto;

namespace TrainMaster.Controllers
{
    [ApiController]
    [Route("api/calendar")]
    public class CalendarController : ControllerBase
    {
        private readonly IUnitOfWorkService _uow;

        public CalendarController(IUnitOfWorkService uow)
        {
            _uow = uow;
        }

        [HttpGet("{year:int}/{month:int}")]
        [ProducesResponseType(typeof(IEnumerable<CalendarItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByMonth([FromRoute] int year, [FromRoute] int month)
        {
            var result = await _uow.CalendarService.GetByMonth(year, month);
            return Ok(result.Data);
        }
    }
}
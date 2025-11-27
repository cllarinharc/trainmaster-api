using Microsoft.AspNetCore.Mvc;
using TrainMaster.Application.UnitOfWork;
using TrainMaster.Domain.Dto;

namespace TrainMaster.Controllers
{
    [ApiController]
    [Route("api/courses/activities/progress")]
    [Produces("application/json")]
    public class CourseActivityProgressController : ControllerBase
    {
        private readonly IUnitOfWorkService _uow;

        public CourseActivityProgressController(IUnitOfWorkService uow)
        {
            _uow = uow;
        }

        [HttpPost("complete")]
        public async Task<IActionResult> MarkActivityAsCompleted([FromBody] CourseActivityProgressDto progressDto)
        {
            if (progressDto == null)
                return BadRequest(new { message = "Payload não pode ser nulo." });

            var result = await _uow.CourseActivityProgressService.MarkActivityAsCompleted(progressDto);

            if (result.Success)
                return Ok(result.Data);

            return BadRequest(new { message = result.Message });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateActivityProgress([FromBody] CourseActivityProgressDto progressDto)
        {
            if (progressDto == null)
                return BadRequest(new { message = "Payload não pode ser nulo." });

            var result = await _uow.CourseActivityProgressService.UpdateActivityProgress(progressDto);

            if (result.Success)
                return Ok(result.Data);

            return BadRequest(new { message = result.Message });
        }

        [HttpGet("student/{studentId:int}/activity/{activityId:int}")]
        public async Task<IActionResult> GetActivityProgress([FromRoute] int studentId, [FromRoute] int activityId)
        {
            var result = await _uow.CourseActivityProgressService.GetActivityProgress(studentId, activityId);

            if (result.Success)
                return Ok(result.Data);

            return NotFound(new { message = result.Message });
        }

        [HttpGet("student/{studentId:int}/course/{courseId:int}")]
        public async Task<IActionResult> GetActivitiesProgressByCourse([FromRoute] int studentId, [FromRoute] int courseId)
        {
            var progressList = await _uow.CourseActivityProgressService.GetActivitiesProgressByCourse(studentId, courseId);
            return Ok(progressList);
        }

        [HttpPost("student/{studentId:int}/activity/{activityId:int}/access")]
        public async Task<IActionResult> MarkActivityAsAccessed([FromRoute] int studentId, [FromRoute] int activityId)
        {
            var result = await _uow.CourseActivityProgressService.MarkActivityAsAccessed(studentId, activityId);

            if (result.Success)
                return Ok(result.Data);

            return BadRequest(new { message = result.Message });
        }
    }
}



using Microsoft.AspNetCore.Mvc;
using TrainMaster.Application.UnitOfWork;
using TrainMaster.Domain.Dto;

namespace TrainMaster.Controllers
{
    [ApiController]
    [Route("api/courses/progress")]
    [Produces("application/json")]
    public class CourseProgressController : ControllerBase
    {
        private readonly IUnitOfWorkService _uow;

        public CourseProgressController(IUnitOfWorkService uow)
        {
            _uow = uow;
        }

        [HttpGet("student/{studentId:int}/course/{courseId:int}")]
        public async Task<IActionResult> GetOrCreateProgress([FromRoute] int studentId, [FromRoute] int courseId)
        {
            var result = await _uow.CourseProgressService.GetOrCreateProgress(studentId, courseId);

            if (result.Success)
                return Ok(result.Data);

            return NotFound(new { message = result.Message });
        }

        [HttpGet("student/{studentId:int}/course/{courseId:int}/current")]
        public async Task<IActionResult> GetProgress([FromRoute] int studentId, [FromRoute] int courseId)
        {
            var result = await _uow.CourseProgressService.GetProgress(studentId, courseId);

            if (result.Success)
                return Ok(result.Data);

            return NotFound(new { message = result.Message });
        }

        [HttpPut("student/{studentId:int}/course/{courseId:int}")]
        public async Task<IActionResult> UpdateProgress([FromRoute] int studentId, [FromRoute] int courseId)
        {
            var result = await _uow.CourseProgressService.UpdateProgress(studentId, courseId);

            if (result.Success)
                return Ok(result.Data);

            return BadRequest(new { message = result.Message });
        }

        [HttpGet("student/{studentId:int}")]
        public async Task<IActionResult> GetProgressByStudent([FromRoute] int studentId)
        {
            var progressList = await _uow.CourseProgressService.GetProgressByStudent(studentId);
            return Ok(progressList);
        }

        [HttpGet("course/{courseId:int}")]
        public async Task<IActionResult> GetProgressByCourse([FromRoute] int courseId)
        {
            var progressList = await _uow.CourseProgressService.GetProgressByCourse(courseId);
            return Ok(progressList);
        }
    }
}


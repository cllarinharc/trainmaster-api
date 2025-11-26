using Microsoft.AspNetCore.Mvc;
using TrainMaster.Application.UnitOfWork;
using TrainMaster.Domain.Dto;

namespace TrainMaster.Controllers
{
    [ApiController]
    [Route("api/courses/enrollments")]
    [Produces("application/json")]
    public class CourseEnrollmentController : ControllerBase
    {
        private readonly IUnitOfWorkService _uow;

        public CourseEnrollmentController(IUnitOfWorkService uow)
        {
            _uow = uow;
        }

        [HttpPost]
        public async Task<IActionResult> EnrollStudent([FromBody] CourseEnrollmentDto enrollmentDto)
        {
            if (enrollmentDto == null)
                return BadRequest(new { message = "Payload não pode ser nulo." });

            var result = await _uow.CourseEnrollmentService.EnrollStudent(enrollmentDto);

            if (result.Success)
                return Ok(result);

            return BadRequest(new { message = result.Message });
        }

        [HttpDelete("{enrollmentId:int}")]
        public async Task<IActionResult> CancelEnrollment([FromRoute] int enrollmentId)
        {
            var result = await _uow.CourseEnrollmentService.CancelEnrollment(enrollmentId);

            if (result.Success)
                return Ok(result);

            return BadRequest(new { message = result.Message });
        }

        [HttpGet("student/{studentId:int}")]
        public async Task<IActionResult> GetEnrollmentsByStudent([FromRoute] int studentId)
        {
            var enrollments = await _uow.CourseEnrollmentService.GetEnrollmentsByStudent(studentId);
            return Ok(enrollments);
        }

        [HttpGet("course/{courseId:int}")]
        public async Task<IActionResult> GetEnrollmentsByCourse([FromRoute] int courseId)
        {
            var enrollments = await _uow.CourseEnrollmentService.GetEnrollmentsByCourse(courseId);
            return Ok(enrollments);
        }

        [HttpGet("{enrollmentId:int}")]
        public async Task<IActionResult> GetEnrollmentById([FromRoute] int enrollmentId)
        {
            var result = await _uow.CourseEnrollmentService.GetEnrollmentById(enrollmentId);

            if (!result.Success || result.Data == null)
                return NotFound(new { message = result.Message ?? "Matrícula não encontrada." });

            return Ok(result.Data);
        }
    }
}


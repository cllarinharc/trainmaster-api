using Microsoft.AspNetCore.Mvc;
using TrainMaster.Application.UnitOfWork;

namespace TrainMaster.Controllers
{
    [ApiController]
    [Route("api/course-feedback")]
    public class CourseFeedbackController : ControllerBase
    {
        private readonly IUnitOfWorkService _uow;

        public CourseFeedbackController(IUnitOfWorkService uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var feedbacks = await _uow.CourseFeedbackService.GetAll();
            return Ok(feedbacks);
        }

        [HttpGet("by-course/{courseId:int}")]
        public async Task<IActionResult> GetByCourseId([FromRoute] int courseId)
        {
            var feedbacks = await _uow.CourseFeedbackService.GetByCourseId(courseId);
            return Ok(feedbacks);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using TrainMaster.Application.UnitOfWork;

namespace TrainMaster.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly IUnitOfWorkService _uow;

        public NotificationController(IUnitOfWorkService uow)
        {
            _uow = uow;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var notification = await _uow.NotificationService.Get(id);
            if (notification == null)
                return NotFound(new { message = "Notificação não encontrada." });

            return Ok(notification);
        }
    }
}
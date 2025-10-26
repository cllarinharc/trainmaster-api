using Microsoft.AspNetCore.Mvc;
using TrainMaster.Application.UnitOfWork;
using TrainMaster.Domain.Dto;

namespace TrainMaster.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWorkService _uow;

        public UserController(IUnitOfWorkService uow)
        {
            _uow = uow;
        }

        [HttpPost("adicionar")]
        public async Task<IActionResult> Add([FromBody] UserAddDto user)
        {
            var result = await _uow.UserService.Add(user);
            return Ok(result);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TrainMaster.Application.UnitOfWork;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Controllers
{
    [ApiController]
    [Route("api/departments")]
    [Produces(MediaTypeNames.Application.Json)]
    public class DepartmentController : ControllerBase
    {
        private readonly IUnitOfWorkService _uow;

        public DepartmentController(IUnitOfWorkService uow)
        {
            _uow = uow;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(DepartmentEntity), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] DepartmentEntity department)
        {
            var result = await _uow.DepartmentService.Add(department);
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<DepartmentEntity>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var departments = await _uow.DepartmentService.Get();
            return Ok(departments);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(DepartmentEntity), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _uow.DepartmentService.GetById(id);
            return Ok(result.Data);
        }

        [HttpGet("by-user/{userId:int}")]
        [ProducesResponseType(typeof(DepartmentEntity), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUserId([FromRoute] int userId)
        {
            var result = await _uow.DepartmentService.GetByUserId(userId);
            if (!result.Success)
                return NotFound(new { message = result.Message });

            return Ok(result.Data);
        }

        [HttpPut("{id:int}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] DepartmentEntity department)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != department.Id)
                return BadRequest(new { message = "Body.Id difere do parâmetro de rota." });

            var exists = await _uow.DepartmentService.GetById(id);
            if (!exists.Success)
                return NotFound(new { message = exists.Message });

            var result = await _uow.DepartmentService.Update(department);
            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _uow.DepartmentService.Delete(id);
            return NoContent();
        }
    }
}
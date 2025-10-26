using Microsoft.AspNetCore.Mvc;
using TrainMaster.Application.UnitOfWork;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Controllers
{
    [ApiController]
    [Route("api/profile")]
    public class PessoalProfileController : ControllerBase
    {
        private readonly IUnitOfWorkService _uow;

        public PessoalProfileController(IUnitOfWorkService uow)
        {
            _uow = uow;
        }

        [HttpPost("adicionarPerfil")]
        public async Task<IActionResult> Add([FromBody] PessoalProfileEntity perfil)
        {
            var result = await _uow.ProfilePessoalService.Add(perfil);
            return Ok(result);
        }

        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PessoalProfilePartDto perfil)
        {
            var result = await _uow.ProfilePessoalService.Update(id, perfil);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _uow.ProfilePessoalService.GetById(id);
            return Ok(result);
        }
    }
}
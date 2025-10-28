using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TrainMaster.Application.UnitOfWork;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Controllers
{
    [ApiController]
    [Route("api/faqs")]
    [Produces(MediaTypeNames.Application.Json)]
    public class FaqController : ControllerBase
    {
        private readonly IUnitOfWorkService _uow;

        public FaqController(IUnitOfWorkService uow)
        {
            _uow = uow;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<FaqEntity>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var faqs = await _uow.FaqService.Get();
            return Ok(faqs);
        }
    }
}
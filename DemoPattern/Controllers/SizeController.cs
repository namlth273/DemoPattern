using ApplicationLogicLayer.Features.Sizes;
using DemoPattern.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DemoPattern.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SizeController : Controller
    {
        private readonly IMediator _mediator;

        public SizeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllService.Query());

            return Ok(result);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Add([FromBody]AddService.Command command)
        {
            await _mediator.Send(command);

            return Ok();
        }
    }
}

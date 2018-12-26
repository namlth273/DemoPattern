using ApplicationLogicLayer.Features.Foo;
using DemoPattern.Attributes;
using DomainLogicLayer.Services.Foo;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DemoPattern.Controllers
{
    [Route("api/[controller]/[action]")]
    public class FooController : Controller
    {
        private readonly IMediator _mediator;

        public FooController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllService.Query());

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetById(GetByIdService.Query query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [ValidateModel]
        //[DisableFormValueModelBinding]
        public async Task<IActionResult> Add(AddService.Command command)
        {
            await _mediator.Send(command);

            return Ok();
        }
    }
}

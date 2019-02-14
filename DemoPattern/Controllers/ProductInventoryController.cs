using ApplicationLogicLayer.Features.ProductInventories;
using DemoPattern.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DemoPattern.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ProductInventoryController : Controller
    {
        private readonly IMediator _mediator;

        public ProductInventoryController(IMediator mediator)
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

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Buy([FromBody]BuyService.Command command)
        {
            await _mediator.Send(command);

            return Ok();
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Sell([FromBody]SellService.Command command)
        {
            await _mediator.Send(command);

            return Ok();
        }
    }
}

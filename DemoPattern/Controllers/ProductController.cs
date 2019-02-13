using ApplicationLogicLayer.Features.Products;
using DemoPattern.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DemoPattern.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ProductController : Controller
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllService.Query());

            return Ok(result);
        }

        //[HttpPost]
        //public async Task<IActionResult> GetById(GetByIdService.Query query)
        //{
        //    var result = await _mediator.Send(query);

        //    return Ok(result);
        //}

        [HttpPost]
        [ValidateModel]
        //[DisableFormValueModelBinding]
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

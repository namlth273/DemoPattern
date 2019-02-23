using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLogicLayer.Features.Products
{
    public class SaveService
    {
        public class Command : IRequest<Unit>
        {
            public Guid? Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Command, AddService.Command>();
                CreateMap<Command, UpdateService.Command>();
            }
        }

        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly IMapper _mapper;
            private readonly IMediator _mediator;

            public CommandHandler(IMapper mapper, IMediator mediator)
            {
                _mapper = mapper;
                _mediator = mediator;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.Id == null)
                {
                    await _mediator.Send(_mapper.Map<AddService.Command>(request), cancellationToken);
                }
                else
                {
                    await _mediator.Send(_mapper.Map<UpdateService.Command>(request), cancellationToken);
                }

                return Unit.Value;
            }
        }
    }
}

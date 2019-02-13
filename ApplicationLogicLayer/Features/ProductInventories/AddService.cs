using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Extensions;
using DataAccessLayer;
using DataAccessLayer.Models;
using EntityFramework.DbContextScope.Interfaces;
using MediatR;

namespace ApplicationLogicLayer.Features.ProductInventories
{
    public class AddService
    {
        public class Command : IRequest<Unit>
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Command, ProductInventory>().SetId();
            }
        }

        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly IDbContextScopeFactory _scopeFactory;
            private readonly IMapper _mapper;

            public CommandHandler(IDbContextScopeFactory scopeFactory, IMapper mapper)
            {
                _scopeFactory = scopeFactory;
                _mapper = mapper;
            }
            public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                using (var scope = _scopeFactory.Create())
                {
                    var context = scope.DbContexts.Get<AppDbContext>();

                    var itemToAdd = _mapper.Map<ProductInventory>(request);

                    context.Set<ProductInventory>().Add(itemToAdd);

                    scope.SaveChanges();
                }

                return Task.FromResult(Unit.Value);
            }
        }
    }
}

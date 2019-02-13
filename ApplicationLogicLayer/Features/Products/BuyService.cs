using AutoMapper;
using Core.Extensions;
using DataAccessLayer;
using DataAccessLayer.Models;
using EntityFramework.DbContextScope.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLogicLayer.Features.Products
{
    public class BuyService
    {
        public class Command : IRequest<Unit>
        {
            public Guid ProductId { get; set; }
            public int Quantity { get; set; }
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

                    var productInventory = context.Set<ProductInventory>()
                        .FirstOrDefault(w => w.ProductId == request.ProductId);

                    if (productInventory != null)
                    {
                        productInventory.Quantity += request.Quantity;
                    }
                    else
                    {
                        var itemToAdd = _mapper.Map<ProductInventory>(request);

                        context.Set<ProductInventory>().Add(itemToAdd);
                    }

                    scope.SaveChanges();
                }

                return Task.FromResult(Unit.Value);
            }
        }
    }
}

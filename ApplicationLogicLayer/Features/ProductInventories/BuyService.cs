using AutoMapper;
using Core.Extensions;
using DataAccessLayer;
using DataAccessLayer.Models;
using EntityFramework.DbContextScope.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLogicLayer.Features.ProductInventories
{
    public class BuyService
    {
        public class Command : IRequest<Unit>
        {
            public Guid ProductId { get; set; }
            public int Quantity { get; set; }
            public double BuyPrice { get; set; }
            public double SellPrice { get; set; }
            public Guid ProductColorId { get; set; }
            public Guid ProductSizeId { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Command, ProductInventory>().SetId();
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(r => r.ProductId).NotEqual(Guid.Empty).WithGlobalMessage("Product is required");
                RuleFor(r => r.ProductColorId).NotEqual(Guid.Empty).WithGlobalMessage("Color is required");
                RuleFor(r => r.ProductSizeId).NotEqual(Guid.Empty).WithGlobalMessage("Size is required");
                RuleFor(r => r.Quantity).GreaterThan(0).WithGlobalMessage("Quantity must be greater than 0");
                RuleFor(r => r.BuyPrice).GreaterThan(0).WithGlobalMessage("Buy Price must be greater than 0");
                RuleFor(r => r.SellPrice).GreaterThan(0).WithGlobalMessage("Sell Price must be greater than 0");
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
                        .FirstOrDefault(w => w.ProductId == request.ProductId
                                             && w.ProductColorId == request.ProductColorId
                                             && w.ProductSizeId == request.ProductSizeId);

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

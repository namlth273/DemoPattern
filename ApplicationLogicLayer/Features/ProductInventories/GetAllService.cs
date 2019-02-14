using AutoMapper;
using Core.Common;
using Core.Extensions;
using DataAccessLayer;
using DataAccessLayer.Models;
using EntityFramework.DbContextScope.Interfaces;
using Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLogicLayer.Features.ProductInventories
{
    public class GetAllService
    {
        public class Query : IRequest<List<Result>>
        {

        }

        public class Result : BaseResultModel
        {
            public Guid ProductColorId { get; set; }
            public Guid ProductSizeId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Color { get; set; }
            public string Size { get; set; }
            public int? Quantity { get; set; }
            public double BuyPrice { get; set; }
            public double SellPrice { get; set; }
        }

        public class NestedModel
        {
            public class ProductModel
            {
                public Product Product { get; set; }
                public ProductInventory ProductInventory { get; set; }
                public ProductColor ProductColor { get; set; }
                public ProductSize ProductSize { get; set; }
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<NestedModel.ProductModel, Result>()
                    .ForMember(m => m.Id, o => o.MapFrom(f => f.Product.Id))
                    .ForMember(m => m.Name, o => o.MapFrom(f => f.Product.Name))
                    .ForMember(m => m.Description, o => o.MapFrom(f => f.Product.Description))
                    .ForMember(m => m.Quantity, o => o.MapFrom(f => f.ProductInventory.Quantity))
                    .ForMember(m => m.BuyPrice, o => o.MapFrom(f => f.ProductInventory.BuyPrice))
                    .ForMember(m => m.SellPrice, o => o.MapFrom(f => f.ProductInventory.SellPrice))
                    .ForMember(m => m.ProductColorId, o => o.MapFrom(f => f.ProductColor.Id))
                    .ForMember(m => m.ProductSizeId, o => o.MapFrom(f => f.ProductSize.Id))
                    .ForMember(m => m.Color, o => o.MapFrom(
                        EnumHelper.CreateEnumToStringExpression(
                            (NestedModel.ProductModel e) => e.ProductColor.ColorType)))
                    .ForMember(m => m.Size, o => o.MapFrom(
                        EnumHelper.CreateEnumToStringExpression(
                            (NestedModel.ProductModel e) => e.ProductSize.SizeType)))
                    ;
            }
        }

        public class QueryHandler : IRequestHandler<Query, List<Result>>
        {
            private readonly IMapper _mapper;
            private readonly IDbContextScopeFactory _scopeFactory;

            public QueryHandler(IMapper mapper, IDbContextScopeFactory scopeFactory)
            {
                _mapper = mapper;
                _scopeFactory = scopeFactory;
            }

            public Task<List<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                using (var scope = _scopeFactory.Create())
                {
                    var context = scope.DbContexts.Get<AppDbContext>();

                    var products = (
                        from prod in context.Set<Product>().GetActiveOnly()
                        join inv in context.Set<ProductInventory>().GetActiveOnly() on prod.Id equals inv.ProductId into inv1
                        from inv in inv1.DefaultIfEmpty()
                        join color in context.Set<ProductColor>() on inv.ProductColorId equals color.Id into color1
                        from color in color1.DefaultIfEmpty()
                        join size in context.Set<ProductSize>() on inv.ProductSizeId equals size.Id into size1
                        from size in size1.DefaultIfEmpty()
                        select new NestedModel.ProductModel
                        {
                            ProductInventory = inv,
                            Product = prod,
                            ProductColor = color,
                            ProductSize = size
                        }).Where(w => w.ProductInventory.Quantity > 0)
                        .OrderBy(o => o.Product.Name)
                        .AsEnumerable().Select(_mapper.Map<Result>).ToList();

                    return Task.FromResult(products);
                }
            }
        }
    }
}


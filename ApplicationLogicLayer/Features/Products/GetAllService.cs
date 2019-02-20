using AutoMapper;
using Core.Common;
using DataAccessLayer;
using DataAccessLayer.Models;
using EntityFramework.DbContextScope.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLogicLayer.Features.Products
{
    public class GetAllService
    {
        public class Query : IRequest<List<Result>>
        {

        }

        public class Result : BaseResultModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Product, Result>();
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

                    var products = context.Set<Product>().Select(_mapper.Map<Result>)
                        .OrderBy(o => o.Name)
                        .ToList();

                    return Task.FromResult(products);
                }
            }
        }
    }
}


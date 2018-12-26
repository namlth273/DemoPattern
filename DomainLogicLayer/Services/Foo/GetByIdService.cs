using AutoMapper;
using DataAccessLayer;
using DataAccessLayer.Models;
using EntityFramework.DbContextScope.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DomainLogicLayer.Services.Foo
{
    public class GetByIdService
    {
        public class Query : IRequest<Result>
        {
            public Guid Id { get; set; }
        }

        public class Result
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Bar, Result>();
            }
        }

        public class QueryHandler : IRequestHandler<Query, Result>
        {
            private readonly IMapper _mapper;
            private readonly IDbContextScopeFactory _scopeFactory;

            public QueryHandler(IMapper mapper, IDbContextScopeFactory scopeFactory)
            {
                _mapper = mapper;
                _scopeFactory = scopeFactory;
            }

            public Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                using (var scope = _scopeFactory.Create())
                {
                    var context = scope.DbContexts.Get<AppDbContext>();

                    var result = context.Set<Bar>().Where(w => w.Id == request.Id).AsEnumerable()
                        .Select(_mapper.Map<Result>)
                        .FirstOrDefault();

                    return Task.FromResult(result);
                }
            }
        }
    }
}

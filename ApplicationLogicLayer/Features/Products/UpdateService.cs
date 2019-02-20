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
    public class UpdateService
    {
        public class Command : IRequest<Unit>
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Command, Product>().MapDefaultValuesForUpdate();
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

                    var product = context.Set<Product>()
                        .FirstOrDefault(w => w.Id == request.Id);

                    if (product != null)
                    {
                        _mapper.Map(request, product);
                    }
                    else
                    {
                        //do st else
                        Console.WriteLine("FirstOrDefault return null value");
                    }

                    scope.SaveChanges();
                }

                return Task.FromResult(Unit.Value);
            }
        }
    }
}

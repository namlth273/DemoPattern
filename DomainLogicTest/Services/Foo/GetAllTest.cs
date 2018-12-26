using AutoFixture;
using AutoMapper;
using DataAccessLayer;
using DataAccessLayer.Models;
using DomainLogicLayer.Services.Foo;
using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;
using MediatR;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DomainLogicTest.Services.Foo
{
    public class GetAllTest
    {
        private readonly Fixture _fixture;

        public GetAllTest()
        {
            _fixture = new Fixture();
        }

        //[Fact]
        //public async Task RunOnce()
        //{
        //    var arrangeQuery = It.IsAny<GetAllService.Query>();
        //    var arrangeResult = It.IsAny<List<GetAllService.Result>>();

        //    var mediator = new Mock<IMediator>();
        //    mediator.Setup(s => s.Send(arrangeQuery, It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(arrangeResult);

        //    var result = await mediator.Object.Send(arrangeQuery);

        //    mediator.Verify(v => v.Send(It.IsAny<GetAllService.Query>(), It.IsAny<CancellationToken>()), Times.Once);

        //    Assert.Equal(result, arrangeResult);
        //}

        [Fact]
        public async Task CanGet()
        {
            var allBars = _fixture.Create<List<Bar>>();
            var mScopeFactory = new Mock<IDbContextScopeFactory>();
            var mDbContext = new Mock<AppDbContext>();
            var mContextScope = new Mock<IDbContextScope>();
            var config = new MapperConfiguration(c =>
            {
                c.AddProfile<GetAllService.MappingProfile>();
            });

            var mapper = config.CreateMapper();
            mDbContext.Setup(s => s.Set<Bar>()).ReturnsDbSet(allBars);
            mContextScope.Setup(s => s.DbContexts.Get<AppDbContext>()).Returns(mDbContext.Object);
            mScopeFactory.Setup(s => s.Create(DbContextScopeOption.JoinExisting)).Returns(mContextScope.Object);
            var handler = new GetAllService.QueryHandler(mapper, mScopeFactory.Object);

            var result = await handler.Handle(It.IsAny<GetAllService.Query>(), It.IsAny<CancellationToken>());

            Assert.All(allBars, action => result.All(a => action.Id == a.Id));
        }
    }
}

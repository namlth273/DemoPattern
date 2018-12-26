using ApplicationLogicLayer.Features.Foo;
using AutoFixture;
using AutoMapper;
using DataAccessLayer;
using DataAccessLayer.Models;
using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApplicationLogicTest.Features.Foo
{
    public class AddServiceTest
    {
        private readonly Fixture _fixture;

        public AddServiceTest()
        {
            _fixture = new Fixture();
        }

        //[Fact]
        //public async Task RunOnce()
        //{
        //    var mediator = new Mock<IMediator>();

        //    mediator.Setup(s => s.Send(It.IsAny<AddService.Command>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(Unit.Value);

        //    var result = await mediator.Object.Send(new AddService.Command());

        //    mediator.Verify(v => v.Send(It.IsAny<AddService.Command>(), It.IsAny<CancellationToken>()), Times.Once);

        //    Assert.Equal(result, Unit.Value);
        //}

        [Fact]
        public async Task SaveChangesIsCalled()
        {
            var command = _fixture.Create<AddService.Command>();
            var mScopeFactory = new Mock<IDbContextScopeFactory>();
            var mDbContext = new Mock<AppDbContext>();
            var mContextScope = new Mock<IDbContextScope>();
            var mMapper = new Mock<IMapper>();

            mDbContext.Setup(s => s.Set<Bar>().Add(It.IsAny<Bar>())).Returns(It.IsAny<EntityEntry<Bar>>());
            mMapper.Setup(s => s.Map<Bar>(command)).Returns(_fixture.Create<Bar>());
            mContextScope.Setup(s => s.DbContexts.Get<AppDbContext>()).Returns(mDbContext.Object);
            mScopeFactory.Setup(s => s.Create(DbContextScopeOption.JoinExisting)).Returns(mContextScope.Object);
            var handler = new AddService.CommandHandler(mMapper.Object, mScopeFactory.Object);

            await handler.Handle(command, It.IsAny<CancellationToken>());

            mContextScope.Verify(v => v.SaveChanges(), Times.Once);
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;

using LENA.Application.Behaviors;
using LENA.Application.Contracts.Auditing;
using LENA.Domain.Entity.Common;
using LENA.Domain.Entity.Inventory;

using MediatR;

using Moq;

using Xunit;

namespace LENA.Application.UnitTests.Behaviors
{
    public class AuditingBehaviorTests
    {
        private static readonly DateTime Now = new(2024, 5, 1, 12, 0, 0, DateTimeKind.Utc);

        private sealed record CreateStub(Item Item) : IRequest<Item>, ICreateCommand
        {
            public AuditableEntity AuditableEntity => Item;
        }

        private sealed record UpdateStub(Item Item) : IRequest<Item>, IUpdateCommand
        {
            public AuditableEntity AuditableEntity => Item;
        }

        private sealed record PlainStub(Item Item) : IRequest<Item>;

        private static AuditingBehavior<TRequest, Item> BehaviorFor<TRequest>(string userName = "tester")
            where TRequest : notnull
        {
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.SetupGet(u => u.UserName).Returns(userName);
            return new AuditingBehavior<TRequest, Item>(currentUser.Object, new FakeTimeProvider(Now));
        }

        [Fact]
        public async Task Create_Command_Is_Stamped_With_Current_User_And_Time()
        {
            var item = new Item { Name = "Test", Unit = "ea", CreatedBy = "spoofed", CreateDate = DateTime.MinValue, LastUpdatedBy = "spoofed" };
            var request = new CreateStub(item);

            await BehaviorFor<CreateStub>().Handle(request, _ => Task.FromResult(item), CancellationToken.None);

            Assert.Equal("tester", item.CreatedBy);
            Assert.Equal(Now, item.CreateDate);
            Assert.Null(item.LastUpdatedBy);
            Assert.Null(item.LastUpdatedDate);
        }

        [Fact]
        public async Task Update_Command_Only_Stamps_LastUpdated_Fields()
        {
            var created = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var item = new Item { Name = "Test", Unit = "ea", CreatedBy = "original", CreateDate = created };
            var request = new UpdateStub(item);

            await BehaviorFor<UpdateStub>().Handle(request, _ => Task.FromResult(item), CancellationToken.None);

            Assert.Equal("original", item.CreatedBy);
            Assert.Equal(created, item.CreateDate);
            Assert.Equal("tester", item.LastUpdatedBy);
            Assert.Equal(Now, item.LastUpdatedDate);
        }

        [Fact]
        public async Task Non_Auditable_Request_Is_Passed_Through_Untouched()
        {
            var item = new Item { Name = "Test", Unit = "ea" };
            var request = new PlainStub(item);

            var result = await BehaviorFor<PlainStub>().Handle(request, _ => Task.FromResult(item), CancellationToken.None);

            Assert.Same(item, result);
            Assert.Empty(item.CreatedBy);
        }

        private sealed class FakeTimeProvider : TimeProvider
        {
            private readonly DateTimeOffset _now;
            public FakeTimeProvider(DateTimeOffset now) => _now = now;
            public override DateTimeOffset GetUtcNow() => _now;
        }
    }
}
using System.Threading.Tasks;

using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Application.Exceptions;
using LENA.Infrastructure.Persistence;
using Moq;
using Xunit;

namespace LENA.Application.UnitTests.Repositories
{
    public class BottleRepositoryAuthorizationTests
    {
        private static BottleRepository CreateRepositoryWithUnauthenticatedUser()
        {
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.SetupGet(u => u.UserID).Throws(new UnauthenticatedUserException());

            var connectionFactory = new Mock<IDbConnectionFactory>();
            return new BottleRepository(connectionFactory.Object, currentUser.Object);
        }

        [Fact]
        public async Task GetByIdAsync_Throws_When_User_Is_Not_Resolved()
        {
            var repository = CreateRepositoryWithUnauthenticatedUser();

            await Assert.ThrowsAsync<UnauthenticatedUserException>(() => repository.GetByIdAsync(1));
        }
    }
}

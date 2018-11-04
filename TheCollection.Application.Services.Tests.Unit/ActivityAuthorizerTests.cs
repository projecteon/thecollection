namespace TheCollection.Application.Services.Tests.Unit {
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FakeItEasy;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;
    using Xunit;

    [Trait(nameof(ActivityAuthorizer), "ActivityAuthorizer tests")]
    public class ActivityAuthorizerTests
    {
        public ActivityAuthorizerTests() {
            FakeUser = A.Fake<IApplicationUser>();
            FakeActivity = A.Fake<IActivity>();
            FakeRepository = A.Fake<IGetRepository<IApplicationUser>>();
            A.CallTo(() => FakeRepository.GetItemAsync(A<string>._)).Returns(Task.FromResult(FakeUser));
        }

        public IGetRepository<IApplicationUser> FakeRepository { get; private set; }
        IApplicationUser FakeUser { get; }
        IActivity FakeActivity { get; }

        [Fact]
        public void GivenApplicationUserRepositoryIsNullWhenConstructorIsCalledThenExceptionIsThrown() {
            Assert.Throws<ArgumentNullException>(() => new ActivityAuthorizer(null));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(8)]
        [InlineData(12)]
        public async Task GivenApplicationUserIsNullAndActivityHasValidRolesWhenIsAuthorizedIsCalledThenExceptionIsThrown(int numberOfRoles) {
            var fakeRepository = A.Fake<IGetRepository<IApplicationUser>>();
            A.CallTo(() => FakeRepository.GetItemAsync(A<string>._)).Returns(Task.FromResult<IApplicationUser>(null));
            A.CallTo(() => FakeActivity.ValidRoles).Returns(A.CollectionOfFake<IRole>(numberOfRoles));

            var authorizer = new ActivityAuthorizer(fakeRepository);

            Assert.False(await authorizer.IsAuthorized(FakeActivity));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(8)]
        [InlineData(12)]
        public async Task GiveApplicationUserHasRolesAndActivityIsNullWhenIsAuthorizedIsCalledThenFalseIsReturned(int numberOfRoles) {
            A.CallTo(() => FakeUser.Roles).Returns(A.CollectionOfFake<IRole>(numberOfRoles));

            var authorizer = new ActivityAuthorizer(FakeRepository);

            Assert.False(await authorizer.IsAuthorized(null));
        }

        [Fact]
        public async Task GiveApplicationUserHasNoRolesAndActivityHasNoValidRolesWhenIsAuthorizedIsCalledThenFalseIsReturned() {
            var authorizer = new ActivityAuthorizer(FakeRepository);

            Assert.False(await authorizer.IsAuthorized(FakeActivity));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(8)]
        [InlineData(12)]
        public async Task GiveApplicationUserHasNoRolesAndActivityHasValidRolesWhenIsAuthorizedIsCalledThenFalseIsReturned(int numberOfRoles) {
            A.CallTo(() => FakeActivity.ValidRoles).Returns(A.CollectionOfFake<IRole>(numberOfRoles));

            var authorizer = new ActivityAuthorizer(FakeRepository);

            Assert.False(await authorizer.IsAuthorized(FakeActivity));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(8)]
        [InlineData(12)]
        public async Task GiveApplicationUserHasRolesAndActivityHasNoValidRolesWhenIsAuthorizedIsCalledThenFalseIsReturned(int numberOfRoles) {
            A.CallTo(() => FakeUser.Roles).Returns(A.CollectionOfFake<IRole>(numberOfRoles));

            var authorizer = new ActivityAuthorizer(FakeRepository);

            Assert.False(await authorizer.IsAuthorized(FakeActivity));
        }

        [Fact]
        public async Task GiveApplicationUserHasRolesAndActivityHasValidRolesThatMatchWhenIsAuthorizedIsCalledThenTrueIsReturned() {
            var fakeRoles = A.CollectionOfFake<IRole>(1);
            A.CallTo(() => FakeUser.Roles).Returns(fakeRoles);
            A.CallTo(() => FakeActivity.ValidRoles).Returns(fakeRoles);

            var authorizer = new ActivityAuthorizer(FakeRepository);

            Assert.True(await authorizer.IsAuthorized(FakeActivity));
        }

        [Fact]
        public async Task GiveApplicationUserHasRolesAndActivityHasValidRolesThatDoesNotMatchWhenIsAuthorizedIsCalledThenFalseIsReturned() {
            var fakeRole = A.Fake<IRole>();
            A.CallTo(() => fakeRole.Name).Returns("Role1");
            A.CallTo(() => FakeUser.Roles).Returns(new List<IRole> { fakeRole });
            A.CallTo(() => FakeActivity.ValidRoles).Returns(A.CollectionOfFake<IRole>(1));

            var authorizer = new ActivityAuthorizer(FakeRepository);

            Assert.False(await authorizer.IsAuthorized(FakeActivity));
        }
    }
}

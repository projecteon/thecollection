namespace TheCollection.Application.Services.Tests.Unit {
    using FakeItEasy;
    using TheCollection.Application.Services.Contracts;
    using Xunit;

    [Trait("ActivityAuthorizerTests", "ActivityAuthorizer tests")]
    public class ActivityAuthorizerTests
    {
        public ActivityAuthorizerTests() {
            FakeUser = A.Fake<IApplicationUser>();
            FakeActivity = A.Fake<IActivity>();
        }

        IApplicationUser FakeUser { get; }
        IActivity FakeActivity { get; }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(8)]
        [InlineData(12)]
        public void GiveApplicationUserIsNullAndActivityHasValidRolesWhenIsAuthorizedIsCalledThenFalseIsReturned(int numberOfRoles) {
            A.CallTo(() => FakeActivity.ValidRoles).Returns(A.CollectionOfFake<IRole>(numberOfRoles));

            var authorizer = new ActivityAuthorizer(null);

            Assert.False(authorizer.IsAuthorized(FakeActivity));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(8)]
        [InlineData(12)]
        public void GiveApplicationUserHasRolesAndActivityIsNullWhenIsAuthorizedIsCalledThenFalseIsReturned(int numberOfRoles) {
            A.CallTo(() => FakeUser.Roles).Returns(A.CollectionOfFake<IRole>(numberOfRoles));

            var authorizer = new ActivityAuthorizer(FakeUser);

            Assert.False(authorizer.IsAuthorized(null));
        }

        [Fact]
        public void GiveApplicationUserHasNoRolesAndActivityHasNoValidRolesWhenIsAuthorizedIsCalledThenFalseIsReturned() {
            var authorizer = new ActivityAuthorizer(FakeUser);

            Assert.False(authorizer.IsAuthorized(FakeActivity));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(8)]
        [InlineData(12)]
        public void GiveApplicationUserHasNoRolesAndActivityHasValidRolesWhenIsAuthorizedIsCalledThenFalseIsReturned(int numberOfRoles) {
            A.CallTo(() => FakeActivity.ValidRoles).Returns(A.CollectionOfFake<IRole>(numberOfRoles));

            var authorizer = new ActivityAuthorizer(FakeUser);

            Assert.False(authorizer.IsAuthorized(FakeActivity));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(8)]
        [InlineData(12)]
        public void GiveApplicationUserHasRolesAndActivityHasNoValidRolesWhenIsAuthorizedIsCalledThenFalseIsReturned(int numberOfRoles) {
            A.CallTo(() => FakeUser.Roles).Returns(A.CollectionOfFake<IRole>(numberOfRoles));

            var authorizer = new ActivityAuthorizer(FakeUser);

            Assert.False(authorizer.IsAuthorized(FakeActivity));
        }

        [Fact]
        public void GiveApplicationUserHasRolesAndActivityHasValidRolesThatMatchWhenIsAuthorizedIsCalledThenTrueIsReturned() {
            var fakeRoles = A.CollectionOfFake<IRole>(1);
            A.CallTo(() => FakeUser.Roles).Returns(fakeRoles);
            A.CallTo(() => FakeActivity.ValidRoles).Returns(fakeRoles);

            var authorizer = new ActivityAuthorizer(FakeUser);

            Assert.True(authorizer.IsAuthorized(FakeActivity));
        }

        [Fact]
        public void GiveApplicationUserHasRolesAndActivityHasValidRolesThatDoesNotMatchWhenIsAuthorizedIsCalledThenFalseIsReturned() {
            A.CallTo(() => FakeUser.Roles).Returns(A.CollectionOfFake<IRole>(1));
            A.CallTo(() => FakeActivity.ValidRoles).Returns(A.CollectionOfFake<IRole>(1));

            var authorizer = new ActivityAuthorizer(FakeUser);

            Assert.False(authorizer.IsAuthorized(FakeActivity));
        }
    }
}

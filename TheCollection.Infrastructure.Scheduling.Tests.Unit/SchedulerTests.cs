namespace TheCollection.Infrastructure.Scheduling.Tests.Unit {

    using System.Threading;
    using System.Threading.Tasks;
    using FakeItEasy;
    using NodaTime;
    using TheCollection.Domain.Core.Contracts;
    using Xunit;

    [Trait(nameof(Scheduler), "Unit tests")]
    public class SchedulerTests {
        private IClock Clock { get; } = A.Fake<IClock>();
        private IScheduledTask ScheduledTask { get; } = A.Fake<IScheduledTask>();

        [Fact]
        public void WhenConstructorIsCalledThenClockGetCurrentInstantIsCalledOnce() {
            A.CallTo(() => Clock.GetCurrentInstant()).Returns(Instant.FromUtc(2016, 1, 1, 0, 0));
            A.CallTo(() => ScheduledTask.Schedule).Returns(Period.FromSeconds(0));

            var controller = new Scheduler(ScheduledTask, Clock);

            A.CallTo(() => Clock.GetCurrentInstant()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void WhenConstructorIsCalledThenScheduledTaskScheduelIsCalledOnce() {
            A.CallTo(() => Clock.GetCurrentInstant()).Returns(Instant.FromUtc(2016, 1, 1, 0, 0));
            A.CallTo(() => ScheduledTask.Schedule).Returns(Period.FromSeconds(0));

            var controller = new Scheduler(ScheduledTask, Clock);

            A.CallTo(() => ScheduledTask.Schedule).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task WhenExecuteAsyncIsCalledThenClockGetCurrentInstantIsCalledASecondTime() {
            A.CallTo(() => Clock.GetCurrentInstant()).Returns(Instant.FromUtc(2016, 1, 1, 0, 0));
            A.CallTo(() => ScheduledTask.Schedule).Returns(Period.FromSeconds(0));
            var controller = new Scheduler(ScheduledTask, Clock);

            await controller.ExecuteAsync(new CancellationToken());

            A.CallTo(() => Clock.GetCurrentInstant()).MustHaveHappened(Repeated.Exactly.Twice);
        }

        [Fact]
        public async Task GivenNextRunTimeIsLargerThenCurrentTimeWhenExecuteAsyncIsCalledThenScheduledTaskExecuteAsyncIsNotCalled() {
            A.CallTo(() => Clock.GetCurrentInstant()).Returns(Instant.FromUtc(2016, 1, 1, 0, 0));
            A.CallTo(() => ScheduledTask.Schedule).Returns(Period.FromSeconds(10));
            var controller = new Scheduler(ScheduledTask, Clock);

            await controller.ExecuteAsync(new CancellationToken());

            A.CallTo(() => ScheduledTask.ExecuteAsync(A<CancellationToken>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task GivenNextRunTimeIsSmallerThenCurrentTimeWhenExecuteAsyncIsCalledThenScheduledTaskExecuteAsyncIsOnceWithSameCancellationToken() {
            A.CallTo(() => Clock.GetCurrentInstant()).ReturnsNextFromSequence(Instant.FromUtc(2016, 1, 1, 0, 0), Instant.FromUtc(2016, 1, 1, 0, 1));
            A.CallTo(() => ScheduledTask.Schedule).Returns(Period.FromSeconds(0));
            var controller = new Scheduler(ScheduledTask, Clock);
            var expected = new CancellationToken();

            await controller.ExecuteAsync(expected);

            A.CallTo(() => ScheduledTask.ExecuteAsync(expected)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task GivenNextRunTimeIsSmallerThenCurrentTimeWhenExecuteAsyncIsCalledScheduledTaskScheduelIsCalledASecondTime() {
            A.CallTo(() => Clock.GetCurrentInstant()).ReturnsNextFromSequence(Instant.FromUtc(2016, 1, 1, 0, 0), Instant.FromUtc(2016, 1, 1, 0, 1));
            A.CallTo(() => ScheduledTask.Schedule).Returns(Period.FromSeconds(0));
            var controller = new Scheduler(ScheduledTask, Clock);
            var expected = new CancellationToken();

            await controller.ExecuteAsync(expected);

            A.CallTo(() => ScheduledTask.Schedule).MustHaveHappened(Repeated.Exactly.Twice);
        }

        [Fact]
        public async Task GivenPeriodIsLargerThanClockStepWhenExecuteAsyncIsCalledThenScheduledTaskExecuteAsyncIsNotCalled() {
            A.CallTo(() => Clock.GetCurrentInstant()).ReturnsNextFromSequence(Instant.FromUtc(2016, 1, 1, 0, 0), Instant.FromUtc(2016, 1, 1, 0, 1));
            A.CallTo(() => ScheduledTask.Schedule).Returns(Period.FromMinutes(2));
            var controller = new Scheduler(ScheduledTask, Clock);
            var expected = new CancellationToken();

            await controller.ExecuteAsync(expected);

            A.CallTo(() => ScheduledTask.ExecuteAsync(expected)).MustNotHaveHappened();
        }

        [Fact]
        public async Task GivenPeriodIsLargerThanClockStepWhenExecuteAsyncIsCalledThenNextRunTimeIsCaluclatedWithNewPeriod() {
            A.CallTo(() => Clock.GetCurrentInstant()).ReturnsNextFromSequence(Instant.FromUtc(2016, 1, 1, 0, 0), Instant.FromUtc(2016, 1, 1, 0, 3), Instant.FromUtc(2016, 1, 1, 0, 4));
            A.CallTo(() => ScheduledTask.Schedule).Returns(Period.FromMinutes(2));
            var controller = new Scheduler(ScheduledTask, Clock);
            var expected = new CancellationToken();

            await controller.ExecuteAsync(expected);
            await controller.ExecuteAsync(expected);

            A.CallTo(() => ScheduledTask.ExecuteAsync(expected)).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}

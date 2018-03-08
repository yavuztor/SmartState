using System;
using Xunit;
using SmartState.Builder;
using System.Threading.Tasks;

namespace SmartState.UnitTests.StateMachineBuilder
{
    public class WithEntryActionAsyncShould
    {
        [Fact]
        public void ReturnAStateBuilder()
        {
            // Arrange
            // Act
            var machine = StateMachine<SampleStates, SampleTriggers>.OnInitialState(SampleStates.Draft)
                .Triggering(SampleTriggers.Submit).TransitionsTo(SampleStates.Submitted)
                .OnState(SampleStates.Submitted)
                    .WithEntryActionAsync<SampleStateful>(a => Task.CompletedTask);

            // Assert
            Assert.True(machine is IBuildState<SampleStates, SampleTriggers>);
        }
    }
}

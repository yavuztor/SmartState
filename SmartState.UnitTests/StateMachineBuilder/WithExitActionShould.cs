using System;
using Xunit;
using SmartState.Builder;

namespace SmartState.UnitTests.StateMachineBuilder
{
    public class WithExitActionShould
    {
        [Fact]
        public void ReturnAStateBuilder()
        {
            // Arrange
            // Act
            var machine = StateMachine<SampleStates, SampleTriggers>.OnInitialState(SampleStates.Draft)
                .Triggering(SampleTriggers.Submit).TransitionsTo(SampleStates.Submitted)
                .OnState(SampleStates.Submitted)
                    .WithExitAction<SampleStateful>((a, nextState) => a.ExitActionCalled = true);

            // Assert
            Assert.True(machine is IBuildState<SampleStates, SampleTriggers>);
        }
    }
}

using System;
using Xunit;
using SmartState.Builder;

namespace SmartState.UnitTests.StateMachineBuilder
{
    public class WithEntryActionShould
    {
        [Fact]
        public void ReturnAStateBuilder()
        {
            // Arrange
            // Act
            var machine = StateMachine<SampleStates, SampleTriggers>.OnInitialState(SampleStates.Draft)
                .Triggering(SampleTriggers.Submit).TransitionsTo(SampleStates.Submitted)
                .OnState(SampleStates.Submitted)
                    .WithEntryAction<SampleStateful>((a, prevState) => a.EntryActionCalled = true);

            // Assert
            Assert.True(machine is IBuildState<SampleStates, SampleTriggers>);
        }
    }
}

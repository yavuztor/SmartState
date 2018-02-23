using System;
using Xunit;
using SmartState.Builder;

namespace SmartState.UnitTests.StateMachineBuilder
{
    public class ToStateShould
    {
        [Fact]
        public void ReturnAStateBuilder()
        {
            // Arrange
            var trigger = StateMachine<SampleStates, SampleTriggers>.OnInitialState(SampleStates.Draft).Triggering(SampleTriggers.Submit);

            // Act
            var state = trigger.TransitionsTo(SampleStates.Submitted);

            // Assert
            Assert.True(state is IBuildState<SampleStates, SampleTriggers>);
        }
    }
}

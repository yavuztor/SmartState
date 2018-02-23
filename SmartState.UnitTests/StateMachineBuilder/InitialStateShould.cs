using System;
using Xunit;
using SmartState.Builder;

namespace SmartState.UnitTests.StateMachineBuilder
{
    public class InitialStateShould
    {
        [Fact]
        public void ReturnAStateBuilder()
        {
            // Arrange
            // Act
            var machine = StateMachine<SampleStates, SampleTriggers>.OnInitialState(SampleStates.Draft);

            // Assert
            Assert.True(machine is IBuildState<SampleStates, SampleTriggers>);
        }
    }
}

using System;
using Xunit;
using SmartState.Builder;

namespace SmartState.UnitTests.StateMachineBuilder
{
    public class FromStateShould
    {
        [Fact]
        public void SetTheCurrentStateOfTheBuilder()
        {
            // Arrange
            var machine = StateMachine<SampleStates, SampleTriggers>.OnInitialState(SampleStates.Draft)
                .Triggering(SampleTriggers.Submit).TransitionsTo(SampleStates.Submitted);

            //Act
            var state = machine.OnState(SampleStates.Submitted);

            // Assert
            Assert.True(machine is IBuildState<SampleStates, SampleTriggers>);
            Assert.Equal(SampleStates.Submitted, machine.CurrentState);
        }
    }
}

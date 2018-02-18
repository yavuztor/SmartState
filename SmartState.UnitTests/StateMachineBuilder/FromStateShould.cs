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
            var machine = StateMachine<SampleStatesEnum, SampleTriggersEnum>.InitialState(SampleStatesEnum.Draft)
                .Trigger(SampleTriggersEnum.Submit).TransitsStateTo(SampleStatesEnum.Submitted);

            //Act
            var state = machine.FromState(SampleStatesEnum.Submitted);

            // Assert
            Assert.True(machine is IBuildState<SampleStatesEnum, SampleTriggersEnum>);
            Assert.Equal(SampleStatesEnum.Submitted, machine.CurrentState);
        }
    }
}

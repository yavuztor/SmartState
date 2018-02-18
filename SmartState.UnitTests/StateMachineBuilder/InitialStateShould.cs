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
            var machine = StateMachine<SampleStatesEnum, SampleTriggersEnum>.InitialState(SampleStatesEnum.Draft);

            // Assert
            Assert.True(machine is IBuildState<SampleStatesEnum, SampleTriggersEnum>);
        }
    }
}

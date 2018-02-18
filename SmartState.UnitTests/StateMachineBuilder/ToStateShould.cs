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
            var trigger = StateMachine<SampleStatesEnum, SampleTriggersEnum>.InitialState(SampleStatesEnum.Draft).Trigger(SampleTriggersEnum.Submit);

            // Act
            var state = trigger.TransitsStateTo(SampleStatesEnum.Submitted);

            // Assert
            Assert.True(state is IBuildState<SampleStatesEnum, SampleTriggersEnum>);
        }
    }
}

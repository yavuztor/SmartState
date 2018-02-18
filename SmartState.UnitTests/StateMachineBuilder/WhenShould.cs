using System;
using Xunit;
using SmartState.Builder;

namespace SmartState.UnitTests.StateMachineBuilder
{
    public class WhenShould
    {
        [Fact]
        public void ReturnIBuildTransit()
        {
            // Arrange
            var trigger = StateMachine<SampleStatesEnum, SampleTriggersEnum>.InitialState(SampleStatesEnum.Draft)
                .Trigger(SampleTriggersEnum.Submit);
            
            // Act
            var guarded = trigger.When<SampleStateful>(z => z.StateHistory != null);

            // Assert
            Assert.True(guarded is IBuildTransit<SampleStatesEnum, SampleTriggersEnum>);
        }
    }
}

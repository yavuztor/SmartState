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
            var trigger = StateMachine<SampleStates, SampleTriggers>.OnInitialState(SampleStates.Draft)
                .Triggering(SampleTriggers.Submit);
            
            // Act
            var guarded = trigger.When<SampleStateful>(z => z.Status != null);

            // Assert
            Assert.True(guarded is IBuildTransit<SampleStates, SampleTriggers>);
        }
    }
}

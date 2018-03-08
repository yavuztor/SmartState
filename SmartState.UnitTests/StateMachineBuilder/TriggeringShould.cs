using System;
using Xunit;
using SmartState.Builder;

namespace SmartState.UnitTests.StateMachineBuilder
{
    public class TriggeringShould
    {
        private IBuildInitialState<SampleStates, SampleTriggers> state = StateMachine<SampleStates, SampleTriggers>.OnInitialState(SampleStates.Draft);
        
        [Fact]
        public void ReturnATriggerBuilder()
        {
            // Act
            var trigger = state.Triggering(SampleTriggers.Submit);

            // Assert
            Assert.True(trigger is IBuildTrigger<SampleStates, SampleTriggers>);
        }

        [Fact]
        public void CanRegisterTriggerWithParameters() {
            // Act
            var trigger = state.Triggering(SampleTriggers.SubmitWithComment);

            // Assert
            Assert.True(trigger is IBuildTrigger<SampleStates, SampleTriggers>);
        }

        [Fact]
        public void CanRegisterOverloadedTrigger() {
            // Act
            var trigger = state.Triggering(SampleTriggers.Submit);

            // Assert
            Assert.True(trigger is IBuildTrigger<SampleStates, SampleTriggers>);
        }
    }
}

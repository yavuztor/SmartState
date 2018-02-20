using System;
using Xunit;
using SmartState.Builder;

namespace SmartState.UnitTests.StateMachineBuilder
{
    public class TriggerShould
    {
        private IBuildInitialState<SampleStatesEnum, SampleTriggersEnum> state = StateMachine<SampleStatesEnum, SampleTriggersEnum>.InitialState(SampleStatesEnum.Draft);
        
        [Fact]
        public void ReturnATriggerBuilder()
        {
            // Act
            var trigger = state.Trigger(SampleTriggersEnum.Submit);

            // Assert
            Assert.True(trigger is IBuildTrigger<SampleStatesEnum, SampleTriggersEnum>);
        }

        [Fact]
        public void CanRegisterTriggerWithParameters() {
            // Act
            var trigger = state.Trigger(SampleTriggersEnum.SubmitWithComment);

            // Assert
            Assert.True(trigger is IBuildTrigger<SampleStatesEnum, SampleTriggersEnum>);
        }

        [Fact]
        public void CanRegisterOverloadedTrigger() {
            // Act
            var trigger = state.Trigger(SampleTriggersEnum.Submit);

            // Assert
            Assert.True(trigger is IBuildTrigger<SampleStatesEnum, SampleTriggersEnum>);
        }
    }
}

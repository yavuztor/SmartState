using System;
using Xunit;
using SmartState.Builder;

namespace SmartState.UnitTests.StateMachine
{
    public class TriggerShould
    {
        [Fact]
        public void UpdateStateHistory()
        {
            // Arrange
            var obj = new SampleStateful();

            // Act
            obj.Submit();

            // Assert
            Assert.Equal(SampleStatesEnum.Submitted, obj.StateHistory.CurrentState);
        }

        [Fact]
        public void ThrowInvalidStateException_IfTriggerIsInvalidForState()
        {
            // Arrange
            var obj = new SampleStateful();

            // Assert
            Assert.Throws<InvalidStateException<SampleStatesEnum, SampleTriggersEnum>>(() => obj.Approve());
        }
    }
}

using System;
using Xunit;
using SmartState.Builder;

namespace SmartState.UnitTests.StateMachine
{
    public class TriggerShould
    {
        [Fact]
        public void UpdateStatus()
        {
            // Arrange
            var obj = new SampleStateful(true);

            // Act
            obj.Submit();

            // Assert
            Assert.Equal(SampleStatesEnum.Submitted, obj.Status.CurrentState);
        }

        [Fact]
        public void ThrowInvalidStateException_IfTriggerIsInvalidForState_And_IfThrowsInvalidStateExceptionIsTrue()
        {
            // Arrange
            var obj = new SampleStateful(true);

            // Assert
            Assert.Throws<InvalidStateException<SampleStatesEnum, SampleTriggersEnum>>(() => obj.Approve());
        }

        [Fact]
        public void NotThrowException_IfTriggerIsInvalidForState_But_IfThrowsInvalidStateExceptionIsFalse() 
        {
            // Arrange
            var obj = new SampleStateful(false);
            var state = obj.Status.CurrentState;
            var historyCount = obj.Status.TransitionHistory.Count;

            // Act
            obj.Approve();

            //Assert
            Assert.Equal(state, obj.Status.CurrentState);
            Assert.Equal(historyCount, obj.Status.TransitionHistory.Count);
        }

        [Fact]
        public void ExecuteOnEntryAndOnExitActions() 
        {
            // Arrange
            var obj = new SampleStateful(true);

            // Act
            obj.Submit();

            // Assert
            Assert.True(obj.EntryActionCalled);
            Assert.True(obj.ExitActionCalled);
        }
    }
}

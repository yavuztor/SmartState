using System;
using Xunit;
using SmartState.Builder;

namespace SmartState.UnitTests.StateMachineBuilder
{
    public class OnEntryShould
    {
        [Fact]
        public void ReturnAStateBuilder()
        {
            // Arrange
            // Act
            var machine = StateMachine<SampleStatesEnum, SampleTriggersEnum>.InitialState(SampleStatesEnum.Draft)
                .Trigger(SampleTriggersEnum.Submit).TransitsStateTo(SampleStatesEnum.Submitted)
                .FromState(SampleStatesEnum.Submitted)
                    .OnEntry<SampleStateful>(a => a.EntryActionCalled = true);

            // Assert
            Assert.True(machine is IBuildState<SampleStatesEnum, SampleTriggersEnum>);
        }
    }
}

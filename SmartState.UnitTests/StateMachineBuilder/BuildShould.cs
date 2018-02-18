using System;
using Xunit;
using SmartState.Builder;

namespace SmartState.UnitTests.StateMachineBuilder
{
    public class BuildShould
    {
        [Fact]
        public void BuildASimpleFSM()
        {
            // Arrange
            var machine = StateMachine<SampleStatesEnum, SampleTriggersEnum>
            .InitialState(SampleStatesEnum.Draft)
                .Trigger(SampleTriggersEnum.Submit).TransitsStateTo(SampleStatesEnum.Submitted)
                .Trigger(SampleTriggersEnum.Save).TransitsStateTo(SampleStatesEnum.Draft)
            .FromState(SampleStatesEnum.Submitted)
                .Trigger(SampleTriggersEnum.Approve).TransitsStateTo(SampleStatesEnum.Approved)
                .Trigger(SampleTriggersEnum.Reject).TransitsStateTo(SampleStatesEnum.Rejected)
            .FromState(SampleStatesEnum.Rejected)
                .Trigger(SampleTriggersEnum.Save).TransitsStateTo(SampleStatesEnum.Draft);

            // Act
            var fsm = machine.Build();

            // Assert
            Assert.True(fsm is StateMachine<SampleStatesEnum, SampleTriggersEnum>);
        }
    }
}

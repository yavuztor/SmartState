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
            var machine = StateMachine<SampleStates, SampleTriggers>
            .OnInitialState(SampleStates.Draft)
                .Triggering(SampleTriggers.Submit).TransitionsTo(SampleStates.Submitted)
                .Triggering(SampleTriggers.Save).TransitionsTo(SampleStates.Draft)
            .OnState(SampleStates.Submitted)
                .Triggering(SampleTriggers.Approve).TransitionsTo(SampleStates.Approved)
                .Triggering(SampleTriggers.Reject).TransitionsTo(SampleStates.Rejected)
            .OnState(SampleStates.Rejected)
                .Triggering(SampleTriggers.Save).TransitionsTo(SampleStates.Draft);

            // Act
            var fsm = machine.Build();

            // Assert
            Assert.True(fsm is StateMachine<SampleStates, SampleTriggers>);
        }
    }
}

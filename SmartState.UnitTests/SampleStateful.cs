namespace SmartState.UnitTests {

    using System;
    using SmartState;
    
    public enum SampleStatesEnum {
        Draft,
        Submitted,
        Approved,
        Rejected
    }

    public enum SampleTriggersEnum {
        Submit,
        SubmitWithComment,
        Save,
        Reject,
        Approve,
        Cancel
    }

    public class SampleStateful
    {
        private static SmartState.StateMachine<SampleStatesEnum, SampleTriggersEnum> stateMachine;
        static SampleStateful() 
        {
            stateMachine = SmartState.StateMachine<SampleStatesEnum, SampleTriggersEnum>
                .InitialState(SampleStatesEnum.Draft)
                    .Trigger(SampleTriggersEnum.Submit).TransitsStateTo(SampleStatesEnum.Submitted)
                    .Trigger(SampleTriggersEnum.SubmitWithComment).TransitsStateTo(SampleStatesEnum.Submitted)
                    .Trigger(SampleTriggersEnum.Submit).TransitsStateTo(SampleStatesEnum.Submitted)
                .FromState(SampleStatesEnum.Submitted)
                    .Trigger(SampleTriggersEnum.Approve).TransitsStateTo(SampleStatesEnum.Approved)
                    .Trigger(SampleTriggersEnum.Save).TransitsStateTo(SampleStatesEnum.Draft)
                    .Trigger(SampleTriggersEnum.Reject).TransitsStateTo(SampleStatesEnum.Rejected)
                .FromState(SampleStatesEnum.Rejected)
                    .Trigger(SampleTriggersEnum.Save).TransitsStateTo(SampleStatesEnum.Draft)
                .Build();
        }
        public SampleStateful() 
        {
            StateHistory = new StateHistory<SampleStatesEnum, SampleTriggersEnum>(SampleStatesEnum.Draft);
        }

        public void Submit() {
            stateMachine.Trigger(this.StateHistory, SampleTriggersEnum.Submit, () => {
                Console.WriteLine("Submitted");
            });
        }

        public StateHistory<SampleStatesEnum, SampleTriggersEnum> StateHistory { get; private set; }

        public void SubmitWithComment(string comment) {
            stateMachine.Trigger(this.StateHistory, SampleTriggersEnum.Submit, () => {
                Console.WriteLine($"Submitted with comment '{comment}'");
            });
        }

        public void Submit(string comment) {
            this.SubmitWithComment(comment);
        }

        public void Save() {
            stateMachine.Trigger(this.StateHistory, SampleTriggersEnum.Save, () => {
                Console.WriteLine("Saved");
            });
        }

        public void Reject() {
            stateMachine.Trigger(this.StateHistory, SampleTriggersEnum.Reject, () => {            
                Console.WriteLine("Rejected");
            });
        }

        public void Approve() {
            stateMachine.Trigger(this.StateHistory, SampleTriggersEnum.Approve, () => {
                Console.WriteLine("Approved");
            });
        }
    }
}
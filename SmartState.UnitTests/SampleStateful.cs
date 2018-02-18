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
        protected static SmartState.StateMachine<SampleStatesEnum, SampleTriggersEnum> stateMachine;
        static SampleStateful() 
        {
            stateMachine = SmartState.StateMachine<SampleStatesEnum, SampleTriggersEnum>
                .InitialState(SampleStatesEnum.Draft).OnExit<SampleStateful>(z => z.ExitActionCalled = true)
                    .Trigger(SampleTriggersEnum.Submit)
                        .When<SampleStateful>(z => z.ShouldAllowTransition)
                        .TransitsStateTo(SampleStatesEnum.Submitted)
                    .Trigger(SampleTriggersEnum.SubmitWithComment).TransitsStateTo(SampleStatesEnum.Submitted)

                .FromState(SampleStatesEnum.Submitted).OnEntry<SampleStateful>(z => z.EntryActionCalled = true)
                    .Trigger(SampleTriggersEnum.Approve).TransitsStateTo(SampleStatesEnum.Approved)
                    .Trigger(SampleTriggersEnum.Save).TransitsStateTo(SampleStatesEnum.Draft)
                    .Trigger(SampleTriggersEnum.Reject).TransitsStateTo(SampleStatesEnum.Rejected)

                .FromState(SampleStatesEnum.Rejected)
                    .Trigger(SampleTriggersEnum.Save).TransitsStateTo(SampleStatesEnum.Draft)
                .Build();
        }
        public SampleStateful(bool throwExceptions) 
        {
            stateMachine.ThrowsInvalidStateException = throwExceptions;
            Status = new Status<SampleStatesEnum, SampleTriggersEnum>(SampleStatesEnum.Draft);
        }

        public bool EntryActionCalled { get; set; } = false;

        public bool ExitActionCalled { get; set; } = false;

        public bool ShouldAllowTransition { get; set; } = true;

        public int SubmitCallCount { get; set; } = 0;
        
        public virtual void Submit() {
            stateMachine.Trigger(this, this.Status, SampleTriggersEnum.Submit, () => {
                Console.WriteLine("Submitted");
                SubmitCallCount++;
            });
        }

        public Status<SampleStatesEnum, SampleTriggersEnum> Status { get; private set; }

        public void SubmitWithComment(string comment) {
            stateMachine.Trigger(this, this.Status, SampleTriggersEnum.Submit, () => {
                Console.WriteLine($"Submitted with comment '{comment}'");
            });
        }

        public void Submit(string comment) {
            this.SubmitWithComment(comment);
        }

        public void Save() {
            stateMachine.Trigger(this, this.Status, SampleTriggersEnum.Save, () => {
                Console.WriteLine("Saved");
            });
        }

        public void Reject() {
            stateMachine.Trigger(this, this.Status, SampleTriggersEnum.Reject, () => {            
                Console.WriteLine("Rejected");
            });
        }

        public void Approve() {
            stateMachine.Trigger(this, this.Status, SampleTriggersEnum.Approve, () => {
                Console.WriteLine("Approved");
            });
        }
    }

    public class SampleChild : SampleStateful
    {
        public SampleChild(bool throwExceptions) : base(throwExceptions)
        {
        }

        override public void Submit()
        {
            stateMachine.Trigger("", this.Status, SampleTriggersEnum.Submit, () => {
                Console.WriteLine("Submitted");
                SubmitCallCount++;
            });
        }
    }
}
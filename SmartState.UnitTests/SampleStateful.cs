namespace SmartState.UnitTests {

    using System;
    using SmartState;
    
    public enum SampleStates {
        Draft,
        Submitted,
        Approved,
        Rejected
    }

    public enum SampleTriggers {
        Submit,
        SubmitWithComment,
        Save,
        Reject,
        Approve,
        Cancel
    }

    public class SampleStateful
    {
        protected static SmartState.StateMachine<SampleStates, SampleTriggers> stateMachine;
        static SampleStateful() 
        {
            stateMachine = SmartState.StateMachine<SampleStates, SampleTriggers>
                .OnInitialState(SampleStates.Draft).WithExitAction<SampleStateful>(z => z.ExitActionCalled = true)
                    .Triggering(SampleTriggers.Submit)
                        .When<SampleStateful>(z => z.ShouldAllowTransition)
                        .TransitionsTo(SampleStates.Submitted)
                    .Triggering(SampleTriggers.SubmitWithComment).TransitionsTo(SampleStates.Submitted)

                .OnState(SampleStates.Submitted).WithEntryAction<SampleStateful>(z => z.EntryActionCalled = true)
                    .Triggering(SampleTriggers.Approve).TransitionsTo(SampleStates.Approved)
                    .Triggering(SampleTriggers.Save).TransitionsTo(SampleStates.Draft)
                    .Triggering(SampleTriggers.Reject).TransitionsTo(SampleStates.Rejected)

                .OnState(SampleStates.Rejected)
                    .Triggering(SampleTriggers.Save).TransitionsTo(SampleStates.Draft)
                .Build();
        }
        public SampleStateful(bool throwExceptions) 
        {
            stateMachine.ThrowsInvalidStateException = throwExceptions;
            Status = stateMachine.InitialStatus();
        }

        public Status<SampleStates, SampleTriggers> Status { get; private set; }

        public bool EntryActionCalled { get; set; } = false;

        public bool ExitActionCalled { get; set; } = false;

        public bool ShouldAllowTransition { get; set; } = true;

        public int SubmitCallCount { get; set; } = 0;
        
        public virtual void Submit() {
            stateMachine.Trigger(this, this.Status, SampleTriggers.Submit, () => {
                Console.WriteLine("Submitted");
                SubmitCallCount++;
            });
        }

        public void SubmitWithComment(string comment) {
            stateMachine.Trigger(this, this.Status, SampleTriggers.Submit, () => {
                Console.WriteLine($"Submitted with comment '{comment}'");
            });
        }

        public void Submit(string comment) {
            this.SubmitWithComment(comment);
        }

        public void Save() {
            stateMachine.Trigger(this, this.Status, SampleTriggers.Save, () => {
                Console.WriteLine("Saved");
            });
        }

        public void Reject() {
            stateMachine.Trigger(this, this.Status, SampleTriggers.Reject, () => {            
                Console.WriteLine("Rejected");
            });
        }

        public void Approve() {
            stateMachine.Trigger(this, this.Status, SampleTriggers.Approve, () => {
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
            stateMachine.Trigger("", this.Status, SampleTriggers.Submit, () => {
                Console.WriteLine("Submitted");
                SubmitCallCount++;
            });
        }
    }
}
# SmartState - State machines made more fun

This library aims to make it easy and fun to work with state machines. There are several advantages:

* State transitions are defined in a single place with a fluent api: `StateMachineBuilder`
* State machine itself is immutable and does not maintain current state (There is a better place to do that: `Status`).
* State machine is thread-safe.
* It follows the UML state diagram concepts. So, it is easy to build a state machine from a diagram.
* It does not force you to implement any interface or subclass anything. All you need is a `Status` object, which holds the current state and state transition history. 
* It allows you to run the same state machine with different types.

With SmartState, you can define your state transitions like this:

```csharp
var stateMachine = StateMachine<SampleStatesEnum, SampleTriggersEnum>
    .InitialState(SampleStatesEnum.Draft).OnExit<SampleStateful>(z => z.ExitActionCalled = true)
        .Trigger(SampleTriggersEnum.Submit)
            .When<SampleStateful>(z => z.ShouldAllowTransition)
            .TransitsStateTo(SampleStatesEnum.Submitted)
        .Trigger(SampleTriggersEnum.SubmitWithComment)
            .TransitsStateTo(SampleStatesEnum.Submitted)

    .FromState(SampleStatesEnum.Submitted).OnEntry<SampleStateful>(z => z.EntryActionCalled = true)
        .Trigger(SampleTriggersEnum.Approve)
            .TransitsStateTo(SampleStatesEnum.Approved)
        .Trigger(SampleTriggersEnum.Save)
            .TransitsStateTo(SampleStatesEnum.Draft)
        .Trigger(SampleTriggersEnum.Reject)
            .TransitsStateTo(SampleStatesEnum.Rejected)

    .FromState(SampleStatesEnum.Rejected)
        .Trigger(SampleTriggersEnum.Save)
            .TransitsStateTo(SampleStatesEnum.Draft)
    .Build();
```

Once the state machine is built, it cannot be changed. It can be used many times like this:

```csharp
// ...

public Status<SampleStatesEnum, SampleTriggersEnum> Status { get; private set; }

public void Submit() {
    stateMachine.Trigger(this, this.Status, SampleTriggersEnum.Submit, () => {
        Console.WriteLine("Submitted");
        SubmitCallCount++;
    });
}

// ...
```

The last argument is the action to perform before updating the status. If the transition is not carried out, the action is not performed.

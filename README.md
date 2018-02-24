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
var stateMachine = SmartState.StateMachine<SampleStates, SampleTriggers>
    .OnInitialState(SampleStates.Draft)
        .WithExitAction<SampleStateful>(z => z.ExitActionCalled = true)
        .Triggering(SampleTriggers.Submit)
            .When<SampleStateful>(z => z.ShouldAllowTransition)
            .TransitionsTo(SampleStates.Submitted)
        .Triggering(SampleTriggers.SubmitWithComment).TransitionsTo(SampleStates.Submitted)

    .OnState(SampleStates.Submitted)
        .WithEntryAction<SampleStateful>(z => z.EntryActionCalled = true)
        .Triggering(SampleTriggers.Approve).TransitionsTo(SampleStates.Approved)
        .Triggering(SampleTriggers.Save).TransitionsTo(SampleStates.Draft)
        .Triggering(SampleTriggers.Reject).TransitionsTo(SampleStates.Rejected)

    .OnState(SampleStates.Rejected)
        .Triggering(SampleTriggers.Save).TransitionsTo(SampleStates.Draft)
    .Build();
```

Once the state machine is built, it cannot be changed. It can be used many times like this:

```csharp
// ...

public Status<SampleStates, SampleTriggers> Status { get; private set; }

public void Submit() {
    stateMachine.Trigger(this, this.Status, SampleTriggers.Submit, () => {
        Console.WriteLine("Submitted");
        SubmitCallCount++;
    });
}

// ...
```

Trigger method takes four arguments:
* First argument is the object that will be used to execute the entry/exit actions and guard conditions. In this example, if the current state is `SampleStates.Draft`, the condition defined with `.When<SampleStateful>(z => z.ShouldAllowTransition)` will be evaluated using this argument. If the transition happens, it will also be used to execute the exit action registered with `.WithExitAction<SampleStateful>(z => z.ExitActionCalled = true)`

* Second argument is the status object that will be updated by the state machine. 

* Third argument is the trigger. In this case, it is the `SampleTriggers.Submit` enumeration. 

* The last argument is the action to perform before updating the status. If the transition is not carried out, the action is not performed. This allows implementing GoF state pattern by using a single class. Since the logic for the submit action is wrapped, it will not get executed until state machine will perform a transition. 

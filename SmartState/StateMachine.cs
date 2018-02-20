using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SmartState.Builder;

namespace SmartState
{
    public class StateMachine<TState, TTrigger>
    {
        private readonly ISet<State<TState, TTrigger>> states;
        private readonly State<TState, TTrigger> initialState;

        public StateMachine(ISet<State<TState, TTrigger>> states, State<TState, TTrigger> initialState) {
            this.states = states;
            this.initialState = initialState;
        }

        public bool ThrowsInvalidStateException { get; set; } = false;

        public static IBuildInitialState<TState, TTrigger> InitialState(TState initialState)
        {
            return new StateMachineBuilder<TState, TTrigger>(initialState);
        }

        public void Trigger(object stateful, Status<TState, TTrigger> Status, TTrigger trigger, Action action)
        {
            var oldState = states.First(z => z.Name.Equals(Status.CurrentState));
            var transition = oldState.Transitions.FirstOrDefault(z => z.Trigger.Equals(trigger) && z.Guard(stateful));
            if (transition == null)
            {
                if (this.ThrowsInvalidStateException) 
                    throw new InvalidStateException<TState, TTrigger>(Status.CurrentState, trigger);
                return;
            }

            action();
            
            Status.AddTransition(transition);

            if (!Status.CurrentState.Equals(oldState.Name)) {
                oldState.ExitAction(stateful);
                var newState = states.FirstOrDefault(z => z.Name.Equals(Status.CurrentState));
                newState.EntryAction(stateful);
            }
        }
        public Status<TState, TTrigger> InitialStatus() {
            return new Status<TState, TTrigger>(this.initialState.Name);
        }
    }


    public class InvalidStateException<TState, TTrigger> : Exception 
    {
        public InvalidStateException(TState state, TTrigger trigger) 
            : base($"Current state [{state.ToString()}] does not have trigger [{trigger.ToString()}]")
        {
            this.State = state;
            this.Trigger = trigger;
        }

        public TState State { get; }
        public TTrigger Trigger { get; }
    }
}

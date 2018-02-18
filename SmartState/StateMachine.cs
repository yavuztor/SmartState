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

        public bool ThrowsInvalidStateException { get; set; } = true;

        public static IBuildState<TState, TTrigger> InitialState(TState initialState)
        {
            return new StateMachineBuilder<TState, TTrigger>(initialState);
        }

        public void Trigger(StateHistory<TState, TTrigger> stateHistory, TTrigger trigger, Action action)
        {
            var state = states.First(z => z.Name.Equals(stateHistory.CurrentState));
            var transition = state.Transitions.FirstOrDefault(z => z.Trigger.Equals(trigger));
            if (transition == null)
            {
                if (this.ThrowsInvalidStateException) 
                    throw new InvalidStateException<TState, TTrigger>(stateHistory.CurrentState, trigger);
                return;
            }

            action();
            
            stateHistory.AddTransition(transition);
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

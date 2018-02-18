using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SmartState.Builder
{
    public interface IBuildTransit<TState, TTrigger>
    {
        IBuildState<TState, TTrigger> TransitsStateTo(TState newState);
    }

    public interface IBuildTrigger<TState, TTrigger>:IBuildTransit<TState, TTrigger>
    {
        IBuildTransit<TState, TTrigger> When<T>(Func<T, bool> guard) where T:class;
    }
    
    public interface IBuildState<TState, TTrigger> {
        IBuildTrigger<TState, TTrigger> Trigger(TTrigger trigger);
        IBuildState<TState, TTrigger> FromState(TState fromState);

        IBuildState<TState, TTrigger> OnEntry<T>(Action<T> action) where T:class;

        IBuildState<TState, TTrigger> OnExit<T>(Action<T> action) where T:class;

        TState CurrentState { get; }

        StateMachine<TState, TTrigger> Build();
    }

    public class StateMachineBuilder<TState, TTrigger> : IBuildState<TState, TTrigger>, IBuildTrigger<TState, TTrigger>
    {
        private State<TState, TTrigger> initialState;
        private TState currentState;
        private TTrigger trigger;

        private ISet<State<TState, TTrigger>> states = new HashSet<State<TState, TTrigger>>();

        private ISet<Transition<TState, TTrigger>> currentTransitions = new HashSet<Transition<TState, TTrigger>>();
        private Func<object, bool> triggerGuard;

        private Action<object> entryAction;
        private Action<object> exitAction;

        public StateMachineBuilder(TState initialState) {
            this.currentState = initialState;
        }

        public TState CurrentState => currentState;

        public StateMachine<TState, TTrigger> Build()
        {
            return new StateMachine<TState, TTrigger>(states, initialState);
        }

        public IBuildState<TState, TTrigger> FromState(TState fromState)
        {
            states.Add(new State<TState, TTrigger>(currentState, currentTransitions, entryAction, exitAction));
            if (initialState == null) initialState = states.First();
            currentState = fromState;
            currentTransitions = new HashSet<Transition<TState, TTrigger>>();
            entryAction = null;
            exitAction = null;
            return this;
        }

        public IBuildState<TState, TTrigger> OnEntry<T>(Action<T> action) where T:class
        {
            entryAction = (object o) => 
            {
                if (o is T) action(o as T);
            };
            return this;
        }

        public IBuildState<TState, TTrigger> OnExit<T>(Action<T> action) where T:class
        {
            exitAction = (object o) => 
            {
                if (o is T) action(o as T);
            };
            return this;
        }

        public IBuildState<TState, TTrigger> TransitsStateTo(TState newState)
        {
            currentTransitions.Add(new Transition<TState, TTrigger>(currentState, trigger, newState, triggerGuard));
            triggerGuard = null;
            return this;
        }

        public IBuildTrigger<TState, TTrigger> Trigger(TTrigger trigger)
        {
            this.trigger = trigger;
            return this;
        }

        public IBuildTransit<TState, TTrigger> When<T>(Func<T, bool> guard) where T : class
        {
            // If transition-time object is not expected type, this guard should have no effect.
            this.triggerGuard = (object o) => (o is T) ? guard(o as T) : true;
            return this;
        }
    }
}
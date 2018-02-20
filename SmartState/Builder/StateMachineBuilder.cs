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
    
    public interface IBuildInitialState<TState, TTrigger> {
        IBuildTrigger<TState, TTrigger> Trigger(TTrigger trigger);

        IBuildState<TState, TTrigger> FromState(TState fromState);

        IBuildState<TState, TTrigger> OnExit<T>(Action<T> action) where T:class;

        TState CurrentState { get; }

        StateMachine<TState, TTrigger> Build();
    }

    public interface IBuildState<TState, TTrigger>:IBuildInitialState<TState, TTrigger>
    {
        IBuildState<TState, TTrigger> OnEntry<T>(Action<T> action) where T:class;
    }

    public class StateMachineBuilder<TState, TTrigger> : IBuildState<TState, TTrigger>, IBuildTrigger<TState, TTrigger>
    {
        private State<TState, TTrigger> initialState;
        private TState currentState;
        private TTrigger trigger;

        private ISet<State<TState, TTrigger>> states = new HashSet<State<TState, TTrigger>>();

        private List<Transition<TState, TTrigger>> currentTransitions = new List<Transition<TState, TTrigger>>();
        private Func<object, bool> triggerGuard;

        private List<Action<object>> entryActions = new List<Action<object>>();
        private List<Action<object>> exitActions = new List<Action<object>>();

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
            states.Add(new State<TState, TTrigger>(currentState, currentTransitions, entryActions, exitActions));
            if (initialState == null) initialState = states.First();
            currentState = fromState;
            currentTransitions = new List<Transition<TState, TTrigger>>();
            entryActions = new List<Action<object>>();
            exitActions = new List<Action<object>>();
            return this;
        }

        public IBuildState<TState, TTrigger> OnEntry<T>(Action<T> action) where T:class
        {
            Action<object> entryAction = (object o) => 
            {
                if (o is T) action(o as T);
            };
            entryActions.Add(entryAction);
            return this;
        }

        public IBuildState<TState, TTrigger> OnExit<T>(Action<T> action) where T:class
        {
            Action<object> exitAction = (object o) => 
            {
                if (o is T) action(o as T);
            };
            exitActions.Add(exitAction);
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
            this.triggerGuard = (object o) => (o is T) && guard(o as T);
            return this;
        }
    }
}
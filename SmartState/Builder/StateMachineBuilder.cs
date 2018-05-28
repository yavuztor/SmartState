using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace SmartState.Builder
{
    public class StateMachineBuilder<TState, TTrigger> : IBuildState<TState, TTrigger>, IBuildInitialState<TState, TTrigger>, IBuildTrigger<TState, TTrigger>
    {
        private State<TState, TTrigger> initialState;
        private TState currentState;
        private TTrigger trigger;

        private ISet<State<TState, TTrigger>> states = new HashSet<State<TState, TTrigger>>();

        private List<Transition<TState, TTrigger>> currentTransitions = new List<Transition<TState, TTrigger>>();
        private Func<object, bool> triggerGuard;

        private List<Func<object, TState, Task>> entryActions = new List<Func<object, TState, Task>>();
        private List<Func<object, TState, Task>> exitActions = new List<Func<object, TState, Task>>();

        public StateMachineBuilder(TState initialState) {
            this.currentState = initialState;
        }

        public TState CurrentState => currentState;

        TState IBuildInitialState<TState, TTrigger>.CurrentState => currentState;

        public StateMachine<TState, TTrigger> Build()
        {
            return new StateMachine<TState, TTrigger>(states, initialState);
        }

        public IBuildState<TState, TTrigger> OnState(TState fromState)
        {
            states.Add(new State<TState, TTrigger>(currentState, currentTransitions, entryActions, exitActions));
            if (initialState == null) initialState = states.First();
            currentState = fromState;
            currentTransitions = new List<Transition<TState, TTrigger>>();
            entryActions = new List<Func<object, TState, Task>>();
            exitActions = new List<Func<object, TState, Task>>();
            return this;
        }

        public IBuildState<TState, TTrigger> WithEntryAction<T>(Action<T, TState> action) where T:class
        {
            Func<object, TState, Task> entryAction = (object o, TState previousState) => 
            {
                if (o is T) action(o as T, previousState);
                return Task.CompletedTask;
            };
            entryActions.Add(entryAction);
            return this;
        }

        public IBuildState<TState, TTrigger> WithEntryActionAsync<T>(Func<T, TState, Task> action) where T:class
        {
            Func<object, TState, Task> entryAction = (object o, TState previousState) => 
            {
                return (o is T) ? action(o as T, previousState) : Task.CompletedTask;
            };
            entryActions.Add(entryAction);
            return this;
        }

        public IBuildState<TState, TTrigger> WithExitAction<T>(Action<T, TState> action) where T:class
        {
            Func<object, TState, Task> exitAction = (object o, TState nextState) => 
            {
                if (o is T) action(o as T, nextState);
                return Task.CompletedTask;
            };
            exitActions.Add(exitAction);
            return this;
        }

        public IBuildState<TState, TTrigger> WithExitActionAsync<T>(Func<T, TState, Task> action) where T:class
        {
            Func<object, TState, Task> entryAction = (object o, TState nextState) => 
            {
                return (o is T) ? action(o as T, nextState) : Task.CompletedTask;
            };
            exitActions.Add(entryAction);
            return this;
        }

        public IBuildState<TState, TTrigger> TransitionsTo(TState newState)
        {
            currentTransitions.Add(new Transition<TState, TTrigger>(currentState, trigger, newState, triggerGuard));
            triggerGuard = null;
            return this;
        }

        public IBuildTrigger<TState, TTrigger> Triggering(TTrigger trigger)
        {
            this.trigger = trigger;
            return this;
        }

        public IBuildTransit<TState, TTrigger> When<T>(Func<T, bool> guard) where T : class
        {
            this.triggerGuard = (object o) => (o is T) && guard(o as T);
            return this;
        }

        IBuildInitialState<TState, TTrigger> IBuildInitialState<TState, TTrigger>.WithExitAction<T>(Action<T, TState> action)
        {
            return this.WithExitAction<T>(action);
        }

        IBuildInitialState<TState, TTrigger> IBuildInitialState<TState, TTrigger>.WithExitActionAsync<T>(Func<T, TState, Task> action)
        {
            return this.WithExitActionAsync<T>(action);
        }
    }
}
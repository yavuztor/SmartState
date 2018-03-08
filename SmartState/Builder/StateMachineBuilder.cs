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

        private List<Func<object, Task>> entryActions = new List<Func<object, Task>>();
        private List<Func<object, Task>> exitActions = new List<Func<object, Task>>();

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
            entryActions = new List<Func<object, Task>>();
            exitActions = new List<Func<object, Task>>();
            return this;
        }

        public IBuildState<TState, TTrigger> WithEntryAction<T>(Action<T> action) where T:class
        {
            Func<object, Task> entryAction = (object o) => 
            {
                if (o is T) action(o as T);
                return Task.CompletedTask;
            };
            entryActions.Add(entryAction);
            return this;
        }

        public IBuildState<TState, TTrigger> WithEntryActionAsync<T>(Func<T, Task> action) where T:class
        {
            Func<object, Task> entryAction = (object o) => 
            {
                return (o is T) ? action(o as T) : Task.CompletedTask;
            };
            entryActions.Add(entryAction);
            return this;
        }

        public IBuildState<TState, TTrigger> WithExitAction<T>(Action<T> action) where T:class
        {
            Func<object, Task> exitAction = (object o) => 
            {
                if (o is T) action(o as T);
                return Task.CompletedTask;
            };
            exitActions.Add(exitAction);
            return this;
        }

        public IBuildState<TState, TTrigger> WithExitActionAsync<T>(Func<T, Task> action) where T:class
        {
            Func<object, Task> entryAction = (object o) => 
            {
                return (o is T) ? action(o as T) : Task.CompletedTask;
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

        IBuildInitialState<TState, TTrigger> IBuildInitialState<TState, TTrigger>.WithExitAction<T>(Action<T> action)
        {
            return this.WithExitAction<T>(action);
        }

        IBuildInitialState<TState, TTrigger> IBuildInitialState<TState, TTrigger>.WithExitActionAsync<T>(Func<T, Task> action)
        {
            return this.WithExitActionAsync<T>(action);
        }
    }
}
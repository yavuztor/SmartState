using System;
using System.Collections.Generic;
using SmartState.Builder;

namespace SmartState
{
    public class State<TState, TTrigger>
    {
        public State(TState name, IEnumerable<Transition<TState, TTrigger>> transitions, IEnumerable<Action<object>> entryActions, IEnumerable<Action<object>> exitActions) {
            Name = name;
            Transitions = transitions;
            this.entryActions = entryActions;
            this.exitActions = exitActions;
        }

        public TState Name { get; }
        public IEnumerable<Transition<TState, TTrigger>> Transitions { get; }

        private IEnumerable<Action<object>> entryActions;
        private IEnumerable<Action<object>> exitActions;

        public void EntryAction(object o) 
        { 
            foreach(var action in entryActions) action(o);
        }
        public void ExitAction(object o) 
        { 
            foreach(var action in exitActions) action(o);
        }
    }
}

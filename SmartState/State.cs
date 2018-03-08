using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartState.Builder;

namespace SmartState
{
    public class State<TState, TTrigger>
    {
        public State(TState name, IEnumerable<Transition<TState, TTrigger>> transitions, IEnumerable<Func<object, Task>> entryActions, IEnumerable<Func<object, Task>> exitActions) {
            Name = name;
            Transitions = transitions;
            this.entryActions = entryActions;
            this.exitActions = exitActions;
        }

        public TState Name { get; }
        public IEnumerable<Transition<TState, TTrigger>> Transitions { get; }

        private IEnumerable<Func<object, Task>> entryActions;
        private IEnumerable<Func<object, Task>> exitActions;

        public Task EntryAction(object o) 
        { 
            return Task.WhenAll(entryActions.Select(z=> z(o)));
        }
        public Task ExitAction(object o) 
        { 
            return Task.WhenAll(exitActions.Select(z=> z(o)));
        }
    }
}

using System;
using System.Collections.Generic;
using SmartState.Builder;

namespace SmartState
{
    public class State<TState, TTrigger>
    {
        public State(TState name, IEnumerable<Transition<TState, TTrigger>> transitions, Action<object> entryAction = null, Action<object> exitAction = null) {
            Name = name;
            Transitions = transitions;
            EntryAction = entryAction ?? NoAction;
            ExitAction = exitAction ?? NoAction;
        }

        public TState Name { get; }
        public IEnumerable<Transition<TState, TTrigger>> Transitions { get; }
        public Action<object> EntryAction { get; }
        public Action<object> ExitAction { get; }

        public static void NoAction(object o) {}
    }
}

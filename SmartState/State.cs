using System;
using System.Collections.Generic;
using SmartState.Builder;

namespace SmartState
{
    public class State<TState, TTrigger>
    {
        public State(TState name, ISet<Transition<TState, TTrigger>> transitions) {
            Name = name;
            Transitions = transitions;
        }

        public TState Name { get; }
        public ISet<Transition<TState, TTrigger>> Transitions { get; }
    }
}

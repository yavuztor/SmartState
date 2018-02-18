using System;
using System.Reflection;

namespace SmartState
{
    public class Transition<TState, TTrigger> {
        
        public Transition(TState fromState, TTrigger trigger, TState toState, Func<object, bool> guard) {
            Trigger = trigger;
            ToState = toState;
            FromState = fromState;
            Guard = guard;
        }

        public TState ToState { get; }
        public TState FromState { get; }
        public Func<object, bool> Guard { get; }
        public TTrigger Trigger { get; }
    }
}
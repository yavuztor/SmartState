using System.Reflection;

namespace SmartState
{
    public class Transition<TState, TTrigger> {
        
        public Transition(TState fromState, TTrigger trigger, TState toState) {
            Trigger = trigger;
            ToState = toState;
            FromState = fromState;
        }

        public TState ToState { get; }
        public TState FromState { get; }
        public TTrigger Trigger { get; }
    }
}
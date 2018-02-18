using System.Collections.Generic;

namespace SmartState 
{
    public class StateHistory<TState, TTrigger>
    {
        private readonly List<Transition<TState, TTrigger>> transitions;

        public StateHistory(TState initialState) 
        {
            CurrentState = initialState;
            transitions = new List<Transition<TState, TTrigger>>();
        }

        public TState CurrentState { get; private set; }


        public IReadOnlyList<Transition<TState, TTrigger>> TransitionHistory { get; }

        internal void AddTransition(Transition<TState, TTrigger> transition)
        {
            transitions.Add(transition);
            CurrentState = transition.ToState;
        }

    }

}
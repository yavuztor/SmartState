using System.Collections.Generic;

namespace SmartState 
{
    public class Status<TState, TTrigger>
    {
        private readonly List<Transition<TState, TTrigger>> transitions;

        public Status(TState initialState) 
        {
            CurrentState = initialState;
            transitions = new List<Transition<TState, TTrigger>>();
        }

        public TState CurrentState { get; private set; }


        public IReadOnlyList<Transition<TState, TTrigger>> TransitionHistory  => transitions.AsReadOnly();

        internal void AddTransition(Transition<TState, TTrigger> transition)
        {
            transitions.Add(transition);
            CurrentState = transition.ToState;
        }

    }

}
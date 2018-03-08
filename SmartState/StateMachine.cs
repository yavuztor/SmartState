using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using SmartState.Builder;

namespace SmartState
{
    public class StateMachine<TState, TTrigger>
    {
        private readonly ISet<State<TState, TTrigger>> states;
        private readonly State<TState, TTrigger> initialState;

        public StateMachine(ISet<State<TState, TTrigger>> states, State<TState, TTrigger> initialState) {
            this.states = states;
            this.initialState = initialState;
        }

        public bool ThrowsInvalidStateException { get; set; } = false;

        public static IBuildInitialState<TState, TTrigger> OnInitialState(TState initialState)
        {
            return new StateMachineBuilder<TState, TTrigger>(initialState);
        }

        public async Task TriggerAsync(object stateful, Status<TState, TTrigger> status, TTrigger trigger, Func<Task> action)
        {
            var oldState = states.First(z => z.Name.Equals(status.CurrentState));
            var transition = oldState.Transitions.FirstOrDefault(z => z.Trigger.Equals(trigger) && z.Guard(stateful));
            if (transition == null)
            {
                if (this.ThrowsInvalidStateException) 
                    throw new InvalidStateException<TState, TTrigger>(status.CurrentState, trigger);
                return;
            }

            await action();
            
            status.AddTransition(transition);

            if (!status.CurrentState.Equals(oldState.Name)) {
                await oldState.ExitAction(stateful);
                var newState = states.FirstOrDefault(z => z.Name.Equals(status.CurrentState));
                await newState.EntryAction(stateful);
            }
        }

        public void Trigger(object stateful, Status<TState, TTrigger> status, TTrigger trigger, Action action) 
        {
            Func<Task> task = () => { action(); return Task.CompletedTask; };
            try {
                TriggerAsync(stateful, status, trigger, task).Wait();
            } catch(AggregateException aggreagate) {
                throw aggreagate.InnerExceptions.Last();
            }
            
        }

        public Status<TState, TTrigger> InitialStatus() {
            return new Status<TState, TTrigger>(this.initialState.Name);
        }
    }


    public class InvalidStateException<TState, TTrigger> : Exception 
    {
        public InvalidStateException(TState state, TTrigger trigger) 
            : base($"Current state [{state.ToString()}] does not have trigger [{trigger.ToString()}]")
        {
            this.State = state;
            this.Trigger = trigger;
        }

        public TState State { get; }
        public TTrigger Trigger { get; }
    }
}

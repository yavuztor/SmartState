using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SmartState.Builder
{
    public interface IBuildTransit<TState, TTrigger>
    {
        IBuildState<TState, TTrigger> TransitsStateTo(TState newState);
    }

    public interface IBuildTrigger<TState, TTrigger>:IBuildTransit<TState, TTrigger>
    {
        IBuildTransit<TState, TTrigger> When<T>(Func<T, bool> guard) where T:class;
    }
    
    public interface IBuildInitialState<TState, TTrigger> {
        IBuildTrigger<TState, TTrigger> Trigger(TTrigger trigger);

        IBuildState<TState, TTrigger> FromState(TState fromState);

        IBuildInitialState<TState, TTrigger> OnExit<T>(Action<T> action) where T:class;

        TState CurrentState { get; }

        StateMachine<TState, TTrigger> Build();
    }

    public interface IBuildState<TState, TTrigger>:IBuildInitialState<TState, TTrigger>
    {
        IBuildState<TState, TTrigger> OnEntry<T>(Action<T> action) where T:class;

        new IBuildState<TState, TTrigger> OnExit<T>(Action<T> action) where T:class;
    }
}
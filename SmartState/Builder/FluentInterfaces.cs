using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace SmartState.Builder
{
    public interface IBuildTransit<TState, TTrigger>
    {
        IBuildState<TState, TTrigger> TransitionsTo(TState newState);
    }

    public interface IBuildTrigger<TState, TTrigger>:IBuildTransit<TState, TTrigger>
    {
        IBuildTransit<TState, TTrigger> When<T>(Func<T, bool> guard) where T:class;
    }
    
    public interface IBuildInitialState<TState, TTrigger> {
        IBuildTrigger<TState, TTrigger> Triggering(TTrigger trigger);

        IBuildState<TState, TTrigger> OnState(TState fromState);

        IBuildInitialState<TState, TTrigger> WithExitAction<T>(Action<T, TState> action) where T:class;

        IBuildInitialState<TState, TTrigger> WithExitActionAsync<T>(Func<T, TState, Task> action) where T:class;

        TState CurrentState { get; }

        StateMachine<TState, TTrigger> Build();
    }

    public interface IBuildState<TState, TTrigger>:IBuildInitialState<TState, TTrigger>
    {
        IBuildState<TState, TTrigger> WithEntryAction<T>(Action<T, TState> action) where T:class;

        IBuildState<TState, TTrigger> WithEntryActionAsync<T>(Func<T, TState, Task> action) where T:class;

        new IBuildState<TState, TTrigger> WithExitAction<T>(Action<T, TState> action) where T:class;

        new IBuildState<TState, TTrigger> WithExitActionAsync<T>(Func<T, TState, Task> action) where T:class;
    }
}
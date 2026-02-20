using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Common.UniversalStateMachine
{
    public class StateMachine : IStateSwitcher, IDisposable
    {
        public bool IsDebug = false;

        private Dictionary<Type, State> _states;
        private State _currentState;

        public StateMachine()
        {
            _states = new Dictionary<Type, State>();
        }
        public void SetStartState<State>()
        {
            if (IsDebug) Debug.Log($"Start state - {typeof(State)}");
            _currentState = _states[typeof(State)];
            _currentState.Enter();
        }

        public void SwitchState<StateT>() where StateT : State
        {
            if (IsDebug) Debug.Log($"Switch state to {typeof(StateT)}");
            
            var state = _states[typeof(StateT)];

            _currentState.Exit();
            _currentState = state;
            _currentState.Enter();
        }

        public void AddState<StateT>(StateT instance) where StateT : State
        {
            if (IsDebug) Debug.Log($"Add state {typeof(StateT)}");

            _states.Add(typeof(StateT), instance);
        }

        public void Update() => _currentState.Update();

        public void Dispose()
        {
            _currentState.Exit();
        }
    }
}

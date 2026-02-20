using Assets.Game.Scripts.BattleSystem.Services.SceneDataAccessor;
using Assets.Game.Scripts.BattleSystem.Unit;
using Assets.Game.Scripts.Common.UniversalStateMachine;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.Execution
{
    public class UnitEvents : IUnitEvents
    {
        public event Action OnAllEnemiesDied;
        public event Action OnUnitDied;
        public event Action OnPlayerDied;

        [Inject] private readonly IBattleUnitCollection _battleUnitCollection;

        private ExecuteAbilitiesDependencies _dependencies;
        private IStateSwitcher _stateSwitcher;

        public void Init(ExecuteAbilitiesDependencies dependencies, IStateSwitcher stateSwitcher)
        {
            _dependencies = dependencies;
            _stateSwitcher = stateSwitcher;
        }

        public void Activate()
        {
            SubscribeToUnitsEvents();
        }

        public void Deactivate() => UnSubscribeFromUnitsEvents();

        private void SubscribeToUnitsEvents()
        {
            _battleUnitCollection.Player.Stats.OnDied += OnPlayerDiedHandler;

            foreach (var unit in _battleUnitCollection.Units)
            {
                unit.Stats.OnDied += OnUnitDiedHandler;
            }
        }
        private void UnSubscribeFromUnitsEvents()
        {
            _battleUnitCollection.Player.Stats.OnDied -= OnPlayerDiedHandler;

            foreach (var unit in _battleUnitCollection.Units)
            {
                unit.Stats.OnDied -= OnUnitDiedHandler;
            }
        }

        private void OnUnitDiedHandler(UnitStats stats)
        {
            OnUnitDied?.Invoke();

            if (_battleUnitCollection.Enemies.All(u => u.Stats.IsDead))
                OnAllEnemiesDied?.Invoke();
        }
        private void OnPlayerDiedHandler(UnitStats unit)
        {
            unit.OnDied -= OnPlayerDiedHandler;

            OnPlayerDied?.Invoke();
        }
    }
}

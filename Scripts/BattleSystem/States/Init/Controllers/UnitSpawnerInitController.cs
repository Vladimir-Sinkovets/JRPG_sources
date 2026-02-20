using Assets.Game.Scripts.BattleSystem.Data;
using Assets.Game.Scripts.BattleSystem.Services.BattlePositions;
using Assets.Game.Scripts.BattleSystem.Services.SceneDataAccessor;
using Assets.Game.Scripts.BattleSystem.Services.UnitFactory;
using Assets.Game.Scripts.BattleSystem.Unit;
using Assets.Game.Scripts.Common.UniversalStateMachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.Init
{
    public class UnitSpawnerInitController : IUnitSpawnerInitController
    {
        [Inject] private readonly BattleArgs _args;
        [Inject] private readonly IBattleUnitCollectionModifier _battleUnitCollectionModifier;
        [Inject] private readonly BattlePositionsData _battlePositionsData;
        [Inject] private readonly IBattleUnitFactory _unitFactory;

        public void Activate()
        {
            SpawnUnits();
        }

        public void Deactivate() { }

        public void Init(InitDependencies dependencies, IStateSwitcher stateSwitcher) { }

        private void SpawnUnits()
        {
            var enemyUnits = SpawnSide(_args.EnemyTeamConfigs, _battlePositionsData.EnemyTeamFreePositions, isEnemy: true);

            var playerUnits = SpawnSide(_args.PlayerTeamConfigs, _battlePositionsData.PlayerTeamFreePositions, isEnemy: false);
            var units = new List<BattleUnit>();

            units.AddRange(playerUnits);
            units.AddRange(enemyUnits);

            var player = units.First();

            _battleUnitCollectionModifier.AddPlayer(player);

            foreach (var unit in units)
            {
                _battleUnitCollectionModifier.AddUnit(unit);
            }
        }

        private List<BattleUnit> SpawnSide(CharacterArgs[] configs, IEnumerable<Transform> positions, bool isEnemy)
        {
            var units = new List<BattleUnit>();

            for (int i = 0; i < positions.Count(); i++)
            {
                if (i >= configs.Length || configs[i] == null)
                    continue;

                var unit = _unitFactory.SpawnUnit(configs[i], positions.ElementAt(i).position, isEnemy);

                _battlePositionsData.SetPositionHolder(positions.ElementAt(i), unit);

                units.Add(unit);
            }

            return units;
        }
    }
}

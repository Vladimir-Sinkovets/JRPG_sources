using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Data;
using Assets.Game.Scripts.BattleSystem.Services.BattlePositions;
using Assets.Game.Scripts.BattleSystem.Services.SceneDataAccessor;
using Assets.Game.Scripts.BattleSystem.Services.UnitFactory;
using Assets.Game.Scripts.Locations.Configs.Party;
using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.Abilities
{
    public class SummonAlly : BattleAbility
    {
        [SerializeField] private AllyConfig _allyConfig;

        [Inject] private readonly IBattleUnitFactory _unitFactory;
        [Inject] private readonly IBattleUnitCollectionModifier _battleUnitCollectionModifier;
        [Inject] private readonly BattlePositionsData _battlePositionsData;

        private void OnValidate()
        {
            if (_hasTargetCost)
                _hasTargetCost = false;
        }

        private void Start()
        {
            _description = $"Summon {_allyConfig.Name}";
        }

        public override IEnumerator Execute(AbilityData data)
        {
            _usingCounter++;

            yield return HandleCost(data);

            var characterArgs = new CharacterArgs()
            {
                Stats = _allyConfig.Stats,
                Abilities = _allyConfig.Abilities,
                BehaviourConfig = null,
                Prefab = _allyConfig.Prefab,
            };

            var freeTransform = _battlePositionsData.PlayerTeamFreePositions.FirstOrDefault();

            var unit = _unitFactory.SpawnUnit(characterArgs, freeTransform.position, isEnemy: false);

            _battlePositionsData.SetPositionHolder(freeTransform, unit);

            _battleUnitCollectionModifier.AddUnit(unit);

            yield break;
        }
    }
}
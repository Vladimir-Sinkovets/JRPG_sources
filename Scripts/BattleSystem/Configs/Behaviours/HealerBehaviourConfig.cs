using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Services.SceneDataAccessor;
using Assets.Game.Scripts.BattleSystem.Services.TokensCostCheckers;
using Assets.Game.Scripts.BattleSystem.Unit;
using Assets.Game.Scripts.Common.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.Configs.Behaviours
{
    [CreateAssetMenu(fileName = "Healer_behaviour", menuName = "Battle/Behaviours/HealerBehaviour")]
    public class HealerBehaviourConfig : EnemyBehaviourConfig
    {
        public BattleAbility DamageAbilityPrefab;
        public BattleAbility HealAbilityPrefab;
        public BattleAbility SpecialHealAbilityPrefab;
        [Space]
        public int HealPercentage = 80;
        public int SpecialHealPercentage = 60;

        private void OnValidate()
        {
            if (HealPercentage < SpecialHealPercentage)
                SpecialHealPercentage = HealPercentage;
        }

        public override EnemyBehaviour CreateBehavior(BattleUnit unit, DiContainer container)
        {
            var behaviour = container.InstantiateComponentOnNewGameObject<HealerBehaviour>();

            behaviour.gameObject.transform.SetParent(unit.transform);
            behaviour.gameObject.transform.localPosition = Vector3.zero;

            behaviour.Init(this);

            return behaviour;
        }
    }

    public class HealerBehaviour : EnemyBehaviour
    {
        private BattleAbility _damageAbility;
        private BattleAbility _healAbility;
        private BattleAbility _specialHealAbility;

        [Inject] private IBattleUnitCollection _battleUnitCollection;
        [Inject] private ITokensCostChecker _tokensCostChecker;
        private HealerBehaviourConfig _config;

        public void Init(HealerBehaviourConfig config) => _config = config;

        public override IEnumerable<BattleAbility> GetAbilities()
        {
            yield return _damageAbility;
            yield return _healAbility;
            yield return _specialHealAbility;
        }

        public override IEnumerable<BattleAbility> InstantiateAbilities(BattleUnit unit, DiContainer container)
        {
            _damageAbility = container.InstantiatePrefab(_config.DamageAbilityPrefab, unit.transform)
                .GetComponent<BattleAbility>();

            _healAbility = container.InstantiatePrefab(_config.HealAbilityPrefab, unit.transform)
                .GetComponent<BattleAbility>();

            _specialHealAbility = container.InstantiatePrefab(_config.SpecialHealAbilityPrefab, unit.transform)
                .GetComponent<BattleAbility>();

            return GetAbilities();
        }

        public override void SetAbility(BattleUnit enemy)
        {
            var enemies = _battleUnitCollection.Enemies.Where(u => !u.Stats.IsDead);

            var allies = _battleUnitCollection.Allies.Where(u => !u.Stats.IsDead)
                .ToList();

            var potentialHealTargets = new List<BattleUnit>();
            var potentialSpecialHealTargets = new List<BattleUnit>();

            foreach (var unit in enemies)
            {
                if (unit.Stats.Hp * 100 / unit.Stats.MaxHp <= _config.HealPercentage)
                    potentialHealTargets.Add(unit);

                if (unit.Stats.Hp * 100 / unit.Stats.MaxHp <= _config.SpecialHealPercentage &&
                    _tokensCostChecker.TryFindTokens(enemy, _specialHealAbility.UnitCost, out _) &&
                    _tokensCostChecker.TryFindTokens(unit, _specialHealAbility.TargetCost, out _))
                {
                    potentialSpecialHealTargets.Add(unit);
                }
            }

            if (potentialSpecialHealTargets.Any())
            {
                var count = Mathf.Min(_specialHealAbility.NumberOfTargets, potentialSpecialHealTargets.Count());

                var targets = potentialSpecialHealTargets.GetRandomElements(count);

                enemy.Abilities.SetAbilityForUse(_specialHealAbility, targets);
            }
            else if (potentialHealTargets.Any())
            {
                var count = Mathf.Min(_healAbility.NumberOfTargets, potentialHealTargets.Count());

                var targets = potentialHealTargets.GetRandomElements(count);

                enemy.Abilities.SetAbilityForUse(_healAbility, targets);
            }
            else
            {
                var count = Mathf.Min(_damageAbility.NumberOfTargets, allies.Count());

                var targets = allies.GetRandomElements(count);

                enemy.Abilities.SetAbilityForUse(_damageAbility, targets);
            }
        }
    }
}


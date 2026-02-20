using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Extensions;
using Assets.Game.Scripts.BattleSystem.Services.AbilityAvailabilityCheckers;
using Assets.Game.Scripts.BattleSystem.Services.SceneDataAccessor;
using Assets.Game.Scripts.BattleSystem.Services.TokensCostCheckers;
using Assets.Game.Scripts.BattleSystem.Unit;
using Assets.Game.Scripts.Common.Extensions;
using Mono.Cecil.Cil;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEditor.Playables;
using UnityEngine;
using Zenject;
using static UnityEngine.UI.GridLayoutGroup;

namespace Assets.Game.Scripts.BattleSystem.Configs.Behaviours
{
    [CreateAssetMenu(fileName = "Support_behaviour", menuName = "Battle/Behaviours/SupportBehaviour")]
    public class UniversalBehaviourConfig : EnemyBehaviourConfig
    {
        public BattleAbility DefaultAbility;
        [InlineProperty]
        public BehaviorPriority DefaultPriority;

        [ListDrawerSettings(AlwaysExpanded = true)]
        public List<BehaviorAbility> Abilities;

        private void OnValidate()
        {
            foreach (var ability in Abilities)
            { 
                ability.ShowTargets();
            }
        }

        public override EnemyBehaviour CreateBehavior(BattleUnit unit, DiContainer container)
        {
            var behaviour = container.InstantiateComponentOnNewGameObject<UniversalBehaviour>();

            behaviour.gameObject.transform.SetParent(unit.transform);
            behaviour.gameObject.transform.localPosition = Vector3.zero;

            behaviour.Init(this);

            return behaviour;
        }
    }

    public class UniversalBehaviour : EnemyBehaviour
    {
        private UniversalBehaviourConfig _config;

        [Inject] private IBattleUnitCollection _battleUnitCollection;
        [Inject] private ITokensCostChecker _tokensCostChecker;
        [Inject] private IAbilityAvailabilityChecker _abilityAvailabilityChecker;

        private BattleAbility _defaultAbilityInstance;
        private Dictionary<BehaviorAbility, BattleAbility> _abilityInstances;

        public void Init(UniversalBehaviourConfig config)
        {
            _config = config;

            _abilityInstances = new Dictionary<BehaviorAbility, BattleAbility>();
        }

        public override IEnumerable<BattleAbility> GetAbilities()
        {
            yield return _defaultAbilityInstance;

            foreach (var ability in _config.Abilities)
            {
                yield return _abilityInstances[ability];
            }
        }

        public override IEnumerable<BattleAbility> InstantiateAbilities(BattleUnit unit, DiContainer container)
        {
            _defaultAbilityInstance = container.InstantiatePrefab(_config.DefaultAbility, unit.transform)
                .GetComponent<BattleAbility>();

            yield return _defaultAbilityInstance;

            foreach (var ability in _config.Abilities)
            {
                var abilityInstance = container.InstantiatePrefab(ability.AbilityPrefab, unit.transform)
                    .GetComponent<BattleAbility>();

                _abilityInstances.Add(ability, abilityInstance);

                yield return abilityInstance;
            }
        }

        public override void SetAbility(BattleUnit enemy)
        {
            enemy.Abilities.ClearAbilityForUse();

            var availableAbilitiesWithTargets = GetAvailableAbilitiesWithTargets(enemy);

            if (availableAbilitiesWithTargets.Any())
            {
                var abilityData = availableAbilitiesWithTargets.GetRandomElements(1).First();

                var targets = GetTargetsByPriority(
                    _abilityInstances[abilityData.ability],
                    abilityData.ability.Priority,
                    abilityData.targets);

                enemy.Abilities.SetAbilityForUse(_abilityInstances[abilityData.ability], targets);
            }
            else if (_abilityAvailabilityChecker.IsAvailable(_defaultAbilityInstance, enemy, out var afforableTargtes))
            {
                var targets = GetTargetsByPriority(
                    _defaultAbilityInstance,
                    _config.DefaultPriority,
                    afforableTargtes);

                enemy.Abilities.SetAbilityForUse(_defaultAbilityInstance, targets);
            }
        }

        private List<(BehaviorAbility ability, IEnumerable<BattleUnit> targets)> GetAvailableAbilitiesWithTargets(BattleUnit enemy)
        {
            var availableAbilitiesWithTargets = new List<(BehaviorAbility, IEnumerable<BattleUnit>)>();

            foreach (var ability in _config.Abilities)
            {
                if (IsAvailable(enemy, ability, out var afforableTargtes))
                {
                    availableAbilitiesWithTargets.Add((ability, afforableTargtes));
                }
            }

            return availableAbilitiesWithTargets.ToList();
        }

        private bool IsAvailable(BattleUnit enemy, BehaviorAbility ability, out IEnumerable<BattleUnit> afforableTargtes)
        {
            if (!_abilityAvailabilityChecker.IsAvailable(_abilityInstances[ability], enemy, out afforableTargtes))
                return false;

            var afforableTargtesList = new List<BattleUnit>();

            foreach (var target in afforableTargtes)
            {
                var isTargetMeetConditions = true;

                foreach (var condition in ability.Conditions)
                {
                    switch (condition.Type)
                    {
                        case BehaviorConditionType.HpAbove:
                            if ((target.Stats.Hp * 100) / target.Stats.MaxHp <= condition.Percentage)
                                isTargetMeetConditions = false;
                            break;
                        case BehaviorConditionType.HpLower:
                            if ((target.Stats.Hp * 100) / target.Stats.MaxHp >= condition.Percentage)
                                isTargetMeetConditions = false;
                            break;
                        case BehaviorConditionType.AlreadyHasEffect:

                            break;
                    }

                    if (!isTargetMeetConditions)
                        break;
                }

                if (isTargetMeetConditions)
                    afforableTargtesList.Add(target);
            }

            var numberOfTargets = _abilityInstances[ability].TargetsType switch
            {
                TargetsType.Selected => Math.Min(_abilityInstances[ability].NumberOfTargets, afforableTargtes.Count()),
                TargetsType.All => _battleUnitCollection.Enemies.Count(),
                TargetsType.Enemies => _battleUnitCollection.Allies.Count(),
                TargetsType.Allies => _battleUnitCollection.Units.Count(),
                _ => throw new NotImplementedException(),
            };

            afforableTargtes = afforableTargtesList;

            if (numberOfTargets > afforableTargtesList.Count)
                return false;

            return true;
        }

        private List<BattleUnit> GetTargetsByPriority(BattleAbility ability, BehaviorPriority priority, IEnumerable<BattleUnit> targets)
        {
            var dict = new Dictionary<BehaviorPriorityTarget, Func<BattleUnit, int>>()
            {
                { BehaviorPriorityTarget.HpPercentage, t => (t.Stats.Hp * 100) / t.Stats.MaxHp },
                { BehaviorPriorityTarget.Hp, t => t.Stats.Hp },
                { BehaviorPriorityTarget.MaxHp, t => t.Stats.MaxHp },
                { BehaviorPriorityTarget.Atk, t => t.Stats.Attack },
                { BehaviorPriorityTarget.MAtk, t => t.Stats.MagicalAttack },
                { BehaviorPriorityTarget.Def, t => t.Stats.Defence },
                { BehaviorPriorityTarget.MDef, t => t.Stats.MagicalDefence },
                { BehaviorPriorityTarget.Speed, t => t.Stats.Speed },
                { BehaviorPriorityTarget.AtkSpeed, t => t.Stats.Attack * t.Stats.Speed },
                { BehaviorPriorityTarget.MAtkSpeed, t => t.Stats.MagicalAttack * t.Stats.Speed },
            };

            var targetsCount = Math.Min(ability.NumberOfTargets, targets.Count());

            if (priority.Type == BehaviorPriorityType.Biggest)
                return targets.OrderByDescending(dict[priority.Target])
                    .Take(targetsCount)
                    .ToList();
            else
                return targets.OrderBy(dict[priority.Target])
                    .Take(targetsCount)
                    .ToList();
        }
    }

    [Serializable]
    [DeclareBoxGroup("Ability", HideTitle = true)]
    public class BehaviorAbility
    {
        [OnValueChanged(nameof(ShowTargets))]
        [Group("Ability")] public BattleAbility AbilityPrefab;
        [ListDrawerSettings(AlwaysExpanded = true)]
        [Group("Ability")] public List<BehaviorCondition> Conditions;

        [DisplayAsString]
        [SerializeField] private string TargetsInfo;

        [InlineProperty]
        [Group("Ability")] public BehaviorPriority Priority;

        public void ShowTargets()
        {
            if (AbilityPrefab == null)
            {
                TargetsInfo = string.Empty;

                return;
            }

            if (AbilityPrefab.TargetsType == TargetsType.Selected)
            {
                TargetsInfo = AbilityPrefab.TargetsType.ToString();

                TargetsInfo += " " + AbilityPrefab.AvailableTargetsType;
            }
            else
            {
                TargetsInfo = AbilityPrefab.TargetsType.ToString();
            }
        }
    }

    [Serializable]
    [DeclareBoxGroup("Condition", HideTitle = true)]
    public class BehaviorCondition
    {
        [HideLabel]
        [Group("Condition")]
        public BehaviorConditionType Type;

        [HideLabel]
        [Group("Condition")]
        [Unit("%")]
        [HideIf(nameof(Type), BehaviorConditionType.AlreadyHasEffect)]
        public int Percentage;
    }

    [Serializable]
    public class BehaviorPriority
    {
        [HideLabel]
        public BehaviorPriorityType Type;

        [HideLabel]
        public BehaviorPriorityTarget Target;
    }

    public enum BehaviorPriorityType
    {
        Biggest,
        Lowest
    }

    public enum BehaviorPriorityTarget
    {
        HpPercentage,
        Hp,
        MaxHp,
        Atk,
        MAtk,
        Def,
        MDef,
        Speed,
        AtkSpeed,
        MAtkSpeed,
    }

    public enum BehaviorConditionType
    {
        HpAbove,
        HpLower,
        AlreadyHasEffect,
    }
}
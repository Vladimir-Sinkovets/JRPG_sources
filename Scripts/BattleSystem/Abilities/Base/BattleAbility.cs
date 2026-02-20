using Assets.Game.Scripts.BattleSystem.Configs;
using Assets.Game.Scripts.BattleSystem.Services.Animators;
using Assets.Game.Scripts.BattleSystem.Services.FormulasServices;
using Assets.Game.Scripts.BattleSystem.Services.TokensCostCheckers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.Abilities.Base
{
    public abstract class BattleAbility : MonoBehaviour
    {
        public static List<AbilityCost> EmptyCostList { get; } = new List<AbilityCost>();

        public event Action<AbilityData> OnExecutionEnded;

        [Title("Info")]
        [SerializeField] protected string _name;
        [TextArea(4,6)]
        [SerializeField] protected string _description;
        [SerializeField] protected Sprite _sprite;

        [Title("Target settings")]
        [SerializeField] protected TargetsType _targetsType;

        [ShowIf(nameof(_targetsType), TargetsType.Selected)]
        [SerializeField] protected AvailableTargetsType _availableTargetsType;

        [ShowIf(nameof(_targetsType), TargetsType.Selected)]
        [HideIf(nameof(_availableTargetsType), AvailableTargetsType.Self)]
        [OnValueChanged(nameof(OnNumberOfTargetsChanged))]
        [SerializeField] protected int _numberOfTargets = 1;

        [Title("Cost")]
        [SerializeField] protected bool _hasUnitCost = false;
        [ShowIf(nameof(_hasUnitCost)), SerializeField, TableList(AlwaysExpanded = true)]
        protected List<AbilityCost> _unitCost;

        [SerializeField] protected bool _hasTargetCost = false;
        [ShowIf(nameof(_hasTargetCost)), SerializeField, TableList(AlwaysExpanded = true)]
        protected List<AbilityCost> _targetCost;

        [Inject] protected FormulasService _formulasService;
        [Inject] protected ITokenCombinationAnimator _tokenCombinationAnimator;
        [Inject] protected ITokensCostChecker _tokensCostChecker;
        [Inject] protected TokensConfig _tokensConfig;

        [SerializeField] protected AbilityType _type = AbilityType.Default;

        [SerializeField] protected bool _hasLimit = false;
        [ShowIf(nameof(_hasLimit))]
        [SerializeField] protected int _limit = 1;

        protected int _usingCounter = 0;

        public string Name { get => _name; }
        public virtual string FullDescription
        {
            get
            {
                return _description + '\n' + GetCostDescription();
            }
        }
        public string DescriptionWithoutCost => _description;
        public string CostDescription => GetCostDescription();

        protected string GetCostDescription()
        {
            var result = string.Empty;

            var targetCost = string.Empty;
            foreach (var cost in TargetCost)
                targetCost += $"<sprite name={cost.Token}> x{cost.Amount} ";

            var unitCost = string.Empty;
            foreach (var cost in UnitCost)
                unitCost += $"<sprite name={cost.Token}> x{cost.Amount} ";

            if (!string.IsNullOrEmpty(targetCost))
                result += $"Target cost: {targetCost}";

            if (!string.IsNullOrEmpty(unitCost))
                result += $"\nUnit cost: {unitCost}";
            return result;
        }

        public AvailableTargetsType AvailableTargetsType { get => _availableTargetsType; }
        public TargetsType TargetsType { get => _targetsType; }
        public int NumberOfTargets
        {
            get
            {
                if (_availableTargetsType == AvailableTargetsType.Self)
                    return 1;

                return _numberOfTargets;
            }
        }
        public Sprite Sprite { get => _sprite; }
        protected FormulasService FormulasService { get => _formulasService; }
        public bool HasUnitCost { get => _hasUnitCost; }
        public bool HasTargetCost { get => _hasTargetCost; }
        public List<AbilityCost> UnitCost
        {
            get
            {
                if (_hasUnitCost == false)
                    return EmptyCostList;

                return _unitCost;
            }
        }
        public List<AbilityCost> TargetCost
        {
            get
            {
                if (_hasTargetCost == false)
                    return EmptyCostList;

                return _targetCost;
            }
        }
        public AbilityType Type { get => _type; set => _type = value; }

        public abstract IEnumerator Execute(AbilityData data);

        protected void End(AbilityData data)
        {
            OnExecutionEnded?.Invoke(data);
        }

        protected virtual void OnNumberOfTargetsChanged() { }

        protected virtual IEnumerator HandleCost(AbilityData data)
        {
            if (data.Ability.HasUnitCost)
            {
                if (_tokensCostChecker.TryFindTokens(data.Owner, data.Ability.UnitCost, out var tokens) == false)
                {
                    Debug.LogError($"Unit {{{data.Owner}}} does not have enough tokens for {{{data.Ability}}}");

                    yield break;
                }

                if (tokens.Any())
                {
                    yield return _tokenCombinationAnimator.AnimateCombination(tokens, data.Owner);

                    data.Owner.Tokens.RemoveTokens(tokens);
                }
            }

            if (data.Ability.HasTargetCost)
            {
                foreach (var target in data.Targets)
                {
                    if (_tokensCostChecker.TryFindTokens(target, data.Ability.TargetCost, out var tokens) == false)
                    {
                        Debug.LogError($"Target {{{target}}} does not have enough tokens for {{{data.Ability}}}");

                        yield break;
                    }

                    yield return _tokenCombinationAnimator.AnimateCombination(tokens, target);

                    target.Tokens.RemoveTokens(tokens);
                }
            }
        }

        public bool IsLimitRiched() => _usingCounter >= _limit;
    }
}
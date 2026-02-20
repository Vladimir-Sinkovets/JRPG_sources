using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Extensions;
using Assets.Game.Scripts.BattleSystem.Services.SceneDataAccessor;
using Assets.Game.Scripts.BattleSystem.Services.TargetSelections;
using Assets.Game.Scripts.BattleSystem.Services.TokensCostCheckers;
using Assets.Game.Scripts.BattleSystem.Unit;
using Assets.Game.Scripts.Common.UniversalStateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.TargetSelection
{
    public class TargetSelectorController : ITargetSelector
    {
        public event Action<BattleUnit> OnTargetSelected;
        public IEnumerable<BattleUnit> ClickableUnits { get => _clickableUnits; set => _clickableUnits = value; }

        [Inject] private readonly BattleStateMachineData _data;
        [Inject] private readonly IBattleUnitCollection _battleUnitCollection;
        [Inject] private readonly ITargetSelectionPanel _targetSelectionPanel;
        [Inject] private readonly ITokensCostChecker _tokensCostChecker;
        private IStateSwitcher StateSwitcher;

        private IEnumerable<BattleUnit> _clickableUnits;

        public void Activate()
        {
            var numberOfTargets = _data.selectedAbility.NumberOfTargets;

            Debug.Log($"Target selection: _numberOfTargets = {numberOfTargets}");

            if (_data.selectedAbility.TargetsType == TargetsType.All)
            {
                _clickableUnits = _battleUnitCollection.Units.Where(u => !u.Stats.IsDead);
                _targetSelectionPanel.EnableForGroup(_clickableUnits, groupName: "All");
            }
            else if (_data.selectedAbility.TargetsType == TargetsType.Enemies)
            {
                _clickableUnits = _battleUnitCollection.Enemies.Where(u => !u.Stats.IsDead);
                _targetSelectionPanel.EnableForGroup(_clickableUnits, groupName: "Enemies");
            }
            else if (_data.selectedAbility.TargetsType == TargetsType.Allies)
            {
                _clickableUnits = _battleUnitCollection.Allies.Where(u => !u.Stats.IsDead);
                _targetSelectionPanel.EnableForGroup(_clickableUnits, groupName: "Allies");
            }
            else if (_data.selectedAbility.TargetsType == TargetsType.Selected)
            {
                _clickableUnits = GetClickableUnits(_data.selectedAbility.AvailableTargetsType);
                _targetSelectionPanel.Enable(_clickableUnits, numberOfTargets);
            }

            _targetSelectionPanel.OnTargetsSelected += OnTargetsSelectedHandler;
        }

        public void Deactivate()
        {
            _targetSelectionPanel.Disable();
            _targetSelectionPanel.OnTargetsSelected -= OnTargetsSelectedHandler;
        }

        public void Init(PlayerTargetDependencies dependencies, IStateSwitcher stateSwitcher)
        {
            StateSwitcher = stateSwitcher;
        }

        private IEnumerable<BattleUnit> GetClickableUnits(AvailableTargetsType type)
        {
            var clickable = _battleUnitCollection.GetAvailableUnits(type, _data.selectedUnit);

            clickable = clickable
                .Where(u => _tokensCostChecker.TryFindTokens(u, _data.selectedAbility.TargetCost, out _));

            return clickable
                .Where(u => u.Stats.IsDead == false)
                .OrderBy(u => u.transform.position.x);
        }

        private void OnTargetsSelectedHandler(IEnumerable<BattleUnit> targets)
        {
            _data.selectedUnit.Abilities.SetAbilityForUse(_data.selectedAbility, targets.ToList());

            StateSwitcher.SwitchState<ExecuteAbilitiesState>();
        }
    }
}
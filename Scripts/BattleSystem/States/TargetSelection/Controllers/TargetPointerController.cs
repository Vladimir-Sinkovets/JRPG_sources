using Assets.Game.Scripts.BattleSystem.Services.TargetSelections;
using Assets.Game.Scripts.BattleSystem.Services.UnitPointers;
using Assets.Game.Scripts.BattleSystem.States.TargetSelection.Interfaces;
using Assets.Game.Scripts.BattleSystem.Unit;
using Assets.Game.Scripts.Common.UniversalStateMachine;
using System;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.TargetSelection.Controllers
{
    public class TargetPointerController : ITargetPointerController
    {
        [Inject] private ITargetSelectionPanel _targetSelectionPanel;
        [Inject] private IUnitPointer _unitPointer;

        public void Init(PlayerTargetDependencies dependencies, IStateSwitcher stateSwitcher) { }

        public void Activate()
        {
            _targetSelectionPanel.OnUnitPointed += OnUnitSelectedHandler;
            _targetSelectionPanel.OnUnitUnpointed += OnUnitUnselectedHandler;
        }

        public void Deactivate()
        {
            _targetSelectionPanel.OnUnitPointed -= OnUnitSelectedHandler;
            _targetSelectionPanel.OnUnitUnpointed -= OnUnitUnselectedHandler;

            _unitPointer.Clear();
        }

        private void OnUnitUnselectedHandler(BattleUnit unit)
        {
            _unitPointer.RemovePointer(unit);
        }

        private void OnUnitSelectedHandler(BattleUnit unit)
        {
            _unitPointer.SetPointer(unit);
        }
    }
}

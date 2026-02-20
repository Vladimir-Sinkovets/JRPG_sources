using Assets.Game.Scripts.BattleSystem.States.TargetSelection;
using Assets.Game.Scripts.BattleSystem.States.TargetSelection.Interfaces;
using System.Collections.Generic;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States
{
    public class PlayerTargetDependencies
    {
        [Inject] public ITargetSelector TargetSelector;
        [Inject] public IHighlightingTargetSelectionController HighlightingController;
        [Inject] public IInfoTargetSelectionController InfoController;
        [Inject] public ICancelButtonTargetSelectionController CancelButtonController;
        [Inject] public ITargetPointerController TargetPointerController;

        public IList<IStateController<PlayerTargetDependencies>> Controllers =>
            new List<IStateController<PlayerTargetDependencies>>()
            {
                TargetPointerController,
                TargetSelector,
                HighlightingController,
                InfoController,
                CancelButtonController,
            };
    }
}
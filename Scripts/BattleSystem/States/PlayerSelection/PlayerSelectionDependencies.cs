using Assets.Game.Scripts.BattleSystem.States.PlayerSelection.Interfaces;
using System.Collections.Generic;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.PlayerSelection
{
    public class PlayerSelectionDependencies
    {
        [Inject] public IPanelController PanelController;
        [Inject] public IUnitHighlightingController UnitHighlightingController;
        [Inject] public IInfoPlayerSelectionController InfoController;

        public IList<IStateController<PlayerSelectionDependencies>> List
            => new List<IStateController<PlayerSelectionDependencies>>
            {
                PanelController,
                UnitHighlightingController,
                InfoController,
            };
    }
}

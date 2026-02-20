using Assets.Game.Scripts.BattleSystem.States.EnemySelection.Interfaces;
using System.Collections.Generic;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.EnemySelection
{
    public class EnemySelectionDependencies
    {
        [Inject] public IEnemyAbilityController AbilityController;

        public IList<IStateController<EnemySelectionDependencies>> Controllers =>
            new List<IStateController<EnemySelectionDependencies>>()
            {
                AbilityController,
            };
    }
}
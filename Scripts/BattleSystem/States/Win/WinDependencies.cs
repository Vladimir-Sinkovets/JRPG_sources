using Assets.Game.Scripts.BattleSystem.States.Win.Interfaces;
using System.Collections.Generic;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.Win
{
    public class WinDependencies
    {
        [Inject] public ILoadBackSceneWinController LoadBackSceneController;

        public IList<IStateController<WinDependencies>> Controllers =>
            new List<IStateController<WinDependencies>>()
            {
                LoadBackSceneController,
            };
    }
}
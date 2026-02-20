using Assets.Game.Scripts.BattleSystem.States.Loss.Interfaces;
using System.Collections.Generic;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.States.Loss
{
    public class LossDependencies
    {
        [Inject] public ILoadBackSceneLossController LoadBackScene;

        public IList<IStateController<LossDependencies>> Controllers =>
            new List<IStateController<LossDependencies>>()
            {
                LoadBackScene,
            };
    }
}
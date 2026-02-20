using Assets.Game.Scripts.Common.UniversalStateMachine;

namespace Assets.Game.Scripts.BattleSystem.States.Win
{
    public class WinState : State
    {
        private readonly WinDependencies _dependencies;

        public WinState(IStateSwitcher stateSwitcher, WinDependencies dependencies) : base(stateSwitcher)
        {
            _dependencies = dependencies;

            foreach (var controller in dependencies.Controllers)
                controller.Init(dependencies, stateSwitcher);
        }

        public override void Enter()
        {
            foreach (var controller in _dependencies.Controllers)
                controller.Activate();
        }

        public override void Exit()
        {
            foreach (var controller in _dependencies.Controllers)
                controller.Deactivate();
        }

        public override void Update() { }
    }
}
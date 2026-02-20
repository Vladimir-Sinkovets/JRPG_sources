using Assets.Game.Scripts.Common.UniversalStateMachine;

namespace Assets.Game.Scripts.BattleSystem.States.Loss
{
    public class LossState : State
    {
        private readonly LossDependencies _dependencies;

        public LossState(IStateSwitcher stateSwitcher, LossDependencies dependencies) : base(stateSwitcher)
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
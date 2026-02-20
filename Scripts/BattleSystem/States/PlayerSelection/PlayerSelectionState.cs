using Assets.Game.Scripts.Common.UniversalStateMachine;

namespace Assets.Game.Scripts.BattleSystem.States.PlayerSelection
{
    public class PlayerSelectionState : State
    {
        private readonly PlayerSelectionDependencies _dependencies;

        public PlayerSelectionState(IStateSwitcher stateSwitcher, PlayerSelectionDependencies dependencies) : base(stateSwitcher)
        {
            _dependencies = dependencies;

            foreach (var controller in _dependencies.List)
                controller.Init(_dependencies, StateSwitcher);
        }

        public override void Enter()
        {
            foreach (var controller in _dependencies.List)
                controller.Activate();
        }

        public override void Exit()
        {
            foreach (var controller in _dependencies.List)
                controller.Deactivate();
        }

        public override void Update() { }
    }
}
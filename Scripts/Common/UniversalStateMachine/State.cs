namespace Assets.Game.Scripts.Common.UniversalStateMachine
{
    public abstract class State
    {
        protected readonly IStateSwitcher StateSwitcher;

        protected State(IStateSwitcher stateSwitcher)
        {
            StateSwitcher = stateSwitcher;
        }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update();
    }
}
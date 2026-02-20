namespace Assets.Game.Scripts.Common.UniversalStateMachine
{
    public interface IStateSwitcher
    {
        void SwitchState<StateT>() where StateT : State;
    }
}
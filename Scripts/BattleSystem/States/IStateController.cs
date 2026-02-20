using Assets.Game.Scripts.Common.UniversalStateMachine;

namespace Assets.Game.Scripts.BattleSystem.States
{
    public interface IStateController<TDependencies>
    {
        void Init(TDependencies dependencies, IStateSwitcher stateSwitcher);
        void Activate();
        void Deactivate();
    }
}
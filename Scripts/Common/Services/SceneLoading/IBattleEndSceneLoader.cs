using Assets.Game.Scripts.BattleSystem.Data;

namespace Assets.Game.Scripts.Common.Services.SceneLoading
{
    public interface IBattleEndSceneLoader
    {
        void Load(string sceneName, BattleResults results);
    }
}

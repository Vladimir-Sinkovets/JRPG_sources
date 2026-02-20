using System;

namespace Assets.Game.Scripts.Common.SaveData
{
    [Serializable]
    public class GameMetaData
    {
        public string SceneName;
        public string TimeStamp;
        public byte[] Picture;
    }
}

using Assets.Game.Scripts.BattleSystem.Tokens;
using System;

namespace Assets.Game.Scripts.BattleSystem.Abilities.Base
{
    [Serializable]
    public class AbilityCost
    {
        public Token Token;
        public int Amount;
    }
}

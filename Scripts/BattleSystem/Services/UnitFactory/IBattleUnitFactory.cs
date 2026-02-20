using Assets.Game.Scripts.BattleSystem.Data;
using Assets.Game.Scripts.BattleSystem.Unit;
using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Services.UnitFactory
{
    public interface IBattleUnitFactory
    {
        BattleUnit SpawnUnit(CharacterArgs config, Vector3 position, bool isEnemy);
    }
}
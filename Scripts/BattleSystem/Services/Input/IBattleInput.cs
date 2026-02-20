using System;
using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Services.Input
{
    public interface IBattleInput
    {
        event Action<Vector2> OnPointerDown;
    }
}
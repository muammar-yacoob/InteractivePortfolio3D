using System;
using UnityEngine;

namespace SparkGames.Portfolio3D.Player
{
    public interface IPlayerInput
    {
        Vector2 Movement { get; }
        event Action<bool> CursorVisibilityChanged;
    }
}
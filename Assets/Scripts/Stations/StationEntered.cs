using System.Threading;
using SparkCore.Runtime.Core;
using UnityEngine;

namespace SparkGames.Portfolio3D.Stations
{
    public class StationEntered: MonoEvent
    {
        public readonly string Title;
        public readonly string Dialogue;
        public readonly Sprite Icon;
        public readonly CancellationToken Token;

        public StationEntered(string title, string dialogue, Sprite icon, CancellationToken token = default)
        {
            Title = title;
            Dialogue = dialogue;
            Icon = icon;
            Token = token;
        }
    }
}
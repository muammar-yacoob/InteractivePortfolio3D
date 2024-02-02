using System.Threading;
using SparkCore.Runtime.Core;

namespace SparkGames.Portfolio3D.Stations
{
    public class StationEntered: MonoEvent
    {
        public readonly string Dialogue;
        public readonly CancellationToken Token;

        public StationEntered(string dialogue, CancellationToken token = default)
        {
            Dialogue = dialogue;
            Token = token;
        }
    }
}
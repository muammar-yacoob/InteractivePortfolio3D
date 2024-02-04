using System.Threading;
using SparkCore.Runtime.Core;

namespace SparkGames.Portfolio3D.Stations
{
    public class StationExited: MonoEvent
    {
        public readonly CancellationToken Token;

        public StationExited(CancellationToken token = default)
        {
            Token = token;
        }
    }
}
using System.Threading;
using SparkCore.Runtime.Core;

namespace SparkGames.Portfolio3D.Stations
{
    public class StationEntered: MonoEvent
    {
        public readonly StationInfo StationInfo;
        public readonly CancellationToken Token;

        public StationEntered(StationInfo stationInfo, CancellationToken token = default)
        {
            StationInfo = stationInfo;
            Token = token;
        }
    }
}
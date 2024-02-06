using System.Threading;
using SparkCore.Runtime.Core;

namespace SparkGames.Portfolio3D.Stations
{
    public class StationEntered: MonoEvent
    {
        //public readonly StationInfo StationInfo;
        public readonly Project ProjectData;
        public readonly CancellationToken Token;

        public StationEntered(Project projectData, CancellationToken token = default)
        {
            // StationInfo = stationInfo;
            ProjectData = projectData;
            Token = token;
        }
    }
}
using System.Threading;
using SparkCore.Runtime.Core;

namespace SparkGames.Portfolio3D.Stations
{
    public class StationURL: MonoEvent
    {
        public readonly string URL;
        public readonly CancellationToken Token;

        public StationURL(string url, CancellationToken token = default)
        {
            URL = url;
            Token = token;
        }
    }
}
using Cysharp.Threading.Tasks;

namespace SparkGames.Portfolio3D
{
    public interface ICVLoader
    {
        CVDataModel CVData { get; }
        public UniTask<CVDataModel> DataLoaded { get; }
    }
}
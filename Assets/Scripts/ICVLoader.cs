using Cysharp.Threading.Tasks;

namespace SparkGames.Portfolio3D
{
    public interface ICVLoader
    {
        public UniTask<CVDataModel> GetCVDataAsync();
    }
}
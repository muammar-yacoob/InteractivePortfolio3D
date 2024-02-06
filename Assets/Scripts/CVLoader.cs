using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using SparkCore.Runtime.Injection;

namespace SparkGames.Portfolio3D
{
    [DefaultExecutionOrder(-500)]
    [ServiceProvider(ServiceLifetime.Singleton)]
    public class CVLoader : ICVLoader, IDisposable
    {
        private readonly string jsonFileName = "CV.json";
        private CVDataModel _cvData;
        public CVDataModel CVData => _cvData;

        private UniTaskCompletionSource<CVDataModel> _dataLoadedCompletionSource = new UniTaskCompletionSource<CVDataModel>();
        public UniTask<CVDataModel> DataLoaded => _dataLoadedCompletionSource.Task;

        public CVLoader()
        {
            LoadJsonData().Forget();
        }

        private async UniTaskVoid LoadJsonData()
        {
            string filePath;
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);
                UnityWebRequest request = UnityWebRequest.Get(filePath);
                await request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    _cvData = JsonUtility.FromJson<CVDataModel>(request.downloadHandler.text);
                    _dataLoadedCompletionSource.TrySetResult(_cvData);
                    Debug.Log($"CV data loaded from {filePath}. Total projects: {_cvData.Projects.Count}");
                }
                else
                {
                    Debug.LogError($"Error loading JSON: {request.error}");
                    _dataLoadedCompletionSource.TrySetException(new Exception($"Error loading JSON: {request.error}"));
                }
            }
            else
            {
                filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);
                if (File.Exists(filePath))
                {
                    string jsonData = await System.IO.File.ReadAllTextAsync(filePath);
                    _cvData = JsonUtility.FromJson<CVDataModel>(jsonData);
                    _dataLoadedCompletionSource.TrySetResult(_cvData);
                    Debug.Log($"CV data loaded from {filePath}. Total projects: {_cvData.Projects.Count}");
                }
                else
                {
                    Debug.LogError($"Cannot find JSON file at {filePath}");
                    _dataLoadedCompletionSource.TrySetException(new Exception($"Cannot find JSON file at {filePath}"));
                }
            }
        }

        public void Dispose()
        {
            Resources.UnloadUnusedAssets();
        }
    }
}

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
        private bool _dataLoaded = false; // Flag to indicate if data is loaded
        private UniTaskCompletionSource<bool> _loadingCompleted = new UniTaskCompletionSource<bool>();

        public CVLoader()
        {
            LoadJsonData().Forget();
        }

        private async UniTaskVoid LoadJsonData()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                UnityWebRequest request = UnityWebRequest.Get(filePath);
                await request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.Success)
                {
                    _cvData = JsonUtility.FromJson<CVDataModel>(request.downloadHandler.text);
                    _dataLoaded = true;
                    _loadingCompleted.TrySetResult(true);
                    Debug.Log($"CV data loaded from {filePath}. Total projects: {_cvData.Projects.Count}");
                }
                else
                {
                    Debug.LogError($"Error loading JSON: {request.error}");
                    _loadingCompleted.TrySetException(new Exception($"Error loading JSON: {request.error}"));
                }
            }
            else
            {
                if (File.Exists(filePath))
                {
                    string jsonData = await File.ReadAllTextAsync(filePath);
                    _cvData = JsonUtility.FromJson<CVDataModel>(jsonData);
                    _dataLoaded = true;
                    _loadingCompleted.TrySetResult(true);
                    Debug.Log($"CV data loaded from {filePath}. Total projects: {_cvData.Projects.Count}");
                }
                else
                {
                    Debug.LogError($"Cannot find JSON file at {filePath}");
                    _loadingCompleted.TrySetException(new Exception($"Cannot find JSON file at {filePath}"));
                }
            }
        }

        public async UniTask<CVDataModel> GetCVDataAsync()
        {
            if (!_dataLoaded)
            {
                await _loadingCompleted.Task; // Wait for the loading to complete
            }
            return _cvData; // Return the loaded data
        }

        public void Dispose()
        {
            Resources.UnloadUnusedAssets();
        }
    }
}

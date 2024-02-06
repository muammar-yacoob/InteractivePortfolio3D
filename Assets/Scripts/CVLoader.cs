using System;
using System.IO;
using Cysharp.Threading.Tasks;
using SparkCore.Runtime.Injection;
using UnityEngine;
using UnityEngine.Networking;

namespace SparkGames.Portfolio3D
{
    public interface ICVLoader
    {
        CVDataModel CVData { get; }
    }

    [ServiceProvider]
    public class CVLoader : ICVLoader, IDisposable
    {
        private readonly string jsonFileName = "CV.json";
        private CVDataModel _cvData;
        public CVDataModel CVData => _cvData;

        public CVLoader()
        {
            // Start an asynchronous operation to load the JSON data
            LoadJsonData().Forget(); // Using UniTask's Forget extension method to fire and forget
        }

        private async UniTask LoadJsonData()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                // Use UnityWebRequest for WebGL
                UnityWebRequest request = UnityWebRequest.Get(filePath);
                await request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error loading JSON: " + request.error);
                }
                else
                {
                    // Successfully loaded the JSON file
                    string jsonData = request.downloadHandler.text;
                    _cvData = JsonUtility.FromJson<CVDataModel>(jsonData);
                }
            }
            else
            {
                // Directly read the file content for non-WebGL platforms
                if (File.Exists(filePath))
                {
                    string jsonData = await File.ReadAllTextAsync(filePath);
                    _cvData = JsonUtility.FromJson<CVDataModel>(jsonData);
                }
                else
                {
                    Debug.LogError("Cannot find JSON file at " + filePath);
                }
            }

            if (_cvData == null) Debug.LogError("CVDataModel is null");
        }

        public void Dispose()
        {
            Resources.UnloadUnusedAssets();
        }
    }
}
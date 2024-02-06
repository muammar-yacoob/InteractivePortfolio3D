using System;
using System.IO;
using SparkCore.Runtime.Injection;
using UnityEngine;

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
        private readonly CVDataModel _cvData;
        public CVDataModel CVData => _cvData;

        public CVLoader()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);
            _cvData = JsonFileReader.LoadTextDataFromJson(filePath);

            if (_cvData == null) Debug.LogError("TextDataModel is null");
        }

        public void Dispose()
        {
            Resources.UnloadUnusedAssets();
        }
    }
}
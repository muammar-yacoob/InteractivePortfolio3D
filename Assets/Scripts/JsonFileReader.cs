using System.IO;
using UnityEngine;

namespace SparkGames.Portfolio3D
{
    public class JsonFileReader
    {
        public static CVDataModel LoadTextDataFromJson(string filePath)
        {
            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                return JsonUtility.FromJson<CVDataModel>(dataAsJson);
            }
            else
            {
                Debug.LogError("Cannot find JSON file at " + filePath);
                return null;
            }
        }
    }
}
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SparkGames.Portfolio3D
{
    public class Text3DGenerator : MonoBehaviour
    {
        [SerializeField] private string jsonFileName = "CV.json";
        [SerializeField] private Transform namePoint;
        [SerializeField] private Transform titlePoint;

        private async UniTaskVoid Start()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);
            TextDataModel textData = JsonFileReader.LoadTextDataFromJson(filePath);

            if (textData == null)
            {
                Debug.LogError("TextDataModel is null");
                return;
            }

            Generate3DText(textData.Name, namePoint);
            Generate3DText(textData.JobTitle, titlePoint);
        }

        void Generate3DText(string text, Transform startPoint)
        {
            Debug.Log($"Generating model for {text}...");
            for (int i = 0; i < text.Length; i++)
            {
                char letter = text[i];
                GameObject letterPrefab = Resources.Load<GameObject>($"Letters/{letter}");
                if (letter == ' ') continue;
                if (letterPrefab == null)
                {
                    Debug.LogWarning($"Prefab for letter '{letter}' not found in Resources/Letters/");
                    continue;
                }

                Quaternion rotationCorrection = Quaternion.Euler(0, 180, 0);
                Instantiate(letterPrefab, startPoint.position + new Vector3(i * 0.5f, 0, 0),
                    startPoint.rotation * rotationCorrection, transform);
            }
        }
    }
}
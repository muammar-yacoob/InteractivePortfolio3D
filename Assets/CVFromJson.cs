using System.IO;
using UnityEngine;

namespace SparkGames.Portfolio3D
{
    public class CVFromJson : MonoBehaviour
    {
        public string jsonFileName = "CV.json"; 

        void Start()
        {
            // Load and parse the JSON data
            TextData textData = LoadTextDataFromJson(jsonFileName);

            // Generate 3D text for JobTitle and Name
            Generate3DText(textData.JobTitle, new Vector3(0, 1, 0)); // Example position for JobTitle
            Generate3DText(textData.Name, new Vector3(0, 0, 0)); // Example position for Name
        }

        TextData LoadTextDataFromJson(string fileName)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                return JsonUtility.FromJson<TextData>(dataAsJson);
            }
            else
            {
                Debug.LogError("Cannot find JSON file!");
                return null;
            }
        }

        void Generate3DText(string text, Vector3 startPosition)
        {
            for (int i = 0; i < text.Length; i++)
            {
                char letter = text[i];
                GameObject letterPrefab = Resources.Load<GameObject>($"Letters/{letter}");
                if (letterPrefab != null)
                {
                    // Adjust the rotation here
                    Quaternion rotation = Quaternion.Euler(0, 180, 0); // Flip the letter by rotating 180 degrees around the Y-axis

                    // Instantiate the letter prefab at the specified position with adjusted rotation
                    Instantiate(letterPrefab, startPosition + new Vector3(i * 0.5f, 0, 0), rotation, transform);
                }
                else
                {
                    Debug.LogWarning($"Prefab for letter '{letter}' not found in Resources/Letters/");
                }
            }
        }
    }
}

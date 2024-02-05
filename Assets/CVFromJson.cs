using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

namespace SparkGames.Portfolio3D
{
    public class CVFromJson : MonoBehaviour
    {
        [SerializeField] private Transform jobTitlePoint;
        [FormerlySerializedAs("namePosition")] [SerializeField] private Transform namePoint;
        
        string jsonFileName = "CV.json"; 

        private void OnEnable()
        {
            TextData textData = LoadTextDataFromJson(jsonFileName);

            Generate3DText(textData.JobTitle, jobTitlePoint);
            Generate3DText(textData.Name, namePoint); 
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

        void Generate3DText(string text, Transform startPoint)
        {
            for (int i = 0; i < text.Length; i++)
            {
                char letter = text[i];
                GameObject letterPrefab = Resources.Load<GameObject>($"Letters/{letter}");
                if (letterPrefab != null)
                {
                    Quaternion rotationCorrection = Quaternion.Euler(0, 180, 0);
                    Instantiate(letterPrefab, startPoint.position + new Vector3(i * 0.5f, 0, 0), startPoint.rotation * rotationCorrection, transform);
                }
                else
                {
                    Debug.LogWarning($"Prefab for letter '{letter}' not found in Resources/Letters/");
                }
            }
        }
    }
}

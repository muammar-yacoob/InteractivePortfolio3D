using Cysharp.Threading.Tasks;
using SparkCore.Runtime.Core;
using SparkCore.Runtime.Injection;
using UnityEngine;

namespace SparkGames.Portfolio3D
{
    public class Text3DGenerator : InjectableMonoBehaviour
    {
        [SerializeField] private Transform namePoint;
        [SerializeField] private Transform titlePoint;
        [Inject] private readonly ICVLoader cvLoader;
        private async void Start()
        {
            var cvData = await cvLoader.GetCVDataAsync();
            Generate3DText(cvData.Name, namePoint);
            Generate3DText(cvData.JobTitle, titlePoint);
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
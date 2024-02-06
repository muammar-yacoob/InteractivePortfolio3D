using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SparkGames.Portfolio3D
{
    public class LoadProjects : MonoBehaviour
    {
        [SerializeField] private string jsonFileName = "CV.json";

        private async UniTaskVoid Start()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);
            TextDataModel textData = JsonFileReader.LoadTextDataFromJson(filePath);

            if (textData == null)
            {
                Debug.LogError("TextDataModel is null");
                return;
            }

            foreach (var project in textData.Projects)
            {
                LoadProjectEntry(project);
            }
        }

        void LoadProjectEntry(Project project)
        {
            // Load icon and SFX by name from Resources
            var icon = Resources.Load<Sprite>($"Icons/{project.Icon}");
            var sfx = Resources.Load<AudioClip>($"SFX/{project.SFX}");

            // Instantiate and set up your project entry here
            // Example: Create a new GameObject for each project, assign the icon to a UI Image, etc.
        }
    }
}
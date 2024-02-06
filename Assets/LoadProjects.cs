using SparkCore.Runtime.Core;
using SparkCore.Runtime.Injection;
using UnityEngine;

namespace SparkGames.Portfolio3D
{
    public class LoadProjects : InjectableMonoBehaviour
    {
        [Inject] private readonly ICVLoader cvLoader;
        private void Start()
        {

            foreach (var project in cvLoader.CVData.Projects)
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
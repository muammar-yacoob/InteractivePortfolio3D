using SparkCore.Runtime.Core;
using SparkCore.Runtime.Injection;
using SparkGames.Portfolio3D.Player;
using UnityEngine;
using System.Linq;

namespace SparkGames.Portfolio3D.Stations
{
    [RequireComponent(typeof(BoxCollider))]
    public class Station : InjectableMonoBehaviour
    {
        //[SerializeField] private StationInfo info;
        [SerializeField] private string projectTitle;
        [Inject] private ICVLoader cvLoader;
        private Project projectData;

        protected override void Awake()
        {
            base.Awake();
            projectData = GetProjectByTitle(projectTitle);
            GetComponent<BoxCollider>().isTrigger = true;
        }

        private Project GetProjectByTitle(string title)
        {
            return cvLoader.CVData.Projects.FirstOrDefault(project => project.Title == title);
        }

        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerMovement playerMovement))
            {
                if(projectData == null)
                {
                    Debug.LogWarning($"Project with title {projectTitle} not found in CV data.");
                    return;
                }
                PublishEvent(new StationEntered(projectData));
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerMovement playerMovement))
            {
                PublishEvent(new StationExited());
            }
        }
    }
}
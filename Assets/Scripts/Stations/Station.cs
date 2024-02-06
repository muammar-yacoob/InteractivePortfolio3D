using SparkCore.Runtime.Core;
using SparkCore.Runtime.Injection;
using SparkGames.Portfolio3D.Player;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

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
            GetProjectByTitle().Forget();
            GetComponent<BoxCollider>().isTrigger = true;
        }

        private async UniTask GetProjectByTitle()
        {
            var cvData = await  cvLoader.DataLoaded;
            var projects = cvData.Projects;
            projectData = projects.FirstOrDefault(project => project.Title == projectTitle);
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
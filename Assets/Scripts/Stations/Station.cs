using SparkCore.Runtime.Core;
using SparkGames.Portfolio3D.Player;
using UnityEngine;

namespace SparkGames.Portfolio3D.Stations
{
    [RequireComponent(typeof(BoxCollider))]
    public class Station : InjectableMonoBehaviour
    {
        [SerializeField] [TextArea] private string stationDialogue;
        protected override void Awake()
        {
            base.Awake();
            GetComponent<BoxCollider>().isTrigger = true;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerMovement playerMovement))
            {
                PublishEvent(new StationEntered(stationDialogue));
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
using SparkCore.Runtime.Core;
using Unity.Cinemachine;
using UnityEngine;

namespace SparkGames.Portfolio3D
{
    [RequireComponent(typeof(BoxCollider))]
    public class WhiteboardCameraController : InjectableMonoBehaviour
    {
        [SerializeField] CinemachineCamera whiteboardCam;
        protected override void Awake()
        {
            base.Awake();
            GetComponent<BoxCollider>().isTrigger = true;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player.PlayerMovement playerMovement))
            {
                PublishEvent(new WhiteboardEntered(whiteboardCam));
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player.PlayerMovement playerMovement))
            {
                PublishEvent(new WhiteboardExited(whiteboardCam));
            }
        }
    }

    public class WhiteboardEntered: MonoEvent
    {
        public CinemachineCamera WhiteboardCam { get; }
        public WhiteboardEntered(CinemachineCamera whiteboardCam)
        {
            WhiteboardCam = whiteboardCam;
        }
    }
    public class WhiteboardExited: MonoEvent
    {
        public CinemachineCamera WhiteboardCam { get; }
        public WhiteboardExited(CinemachineCamera whiteboardCam)
        {
            WhiteboardCam = whiteboardCam;
        }
    }
}
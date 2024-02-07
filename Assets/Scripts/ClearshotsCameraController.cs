using SparkCore.Runtime.Core;
using Unity.Cinemachine;
using UnityEngine;

namespace SparkGames.Portfolio3D
{
    public class ClearshotsCameraController : InjectableMonoBehaviour
    {
        [SerializeField] private CinemachineCamera freelookCam;
        [SerializeField] private LayerMask playerLayer;

        private int originalCullingMask;

        private PrioritySettings highPriority;
        private PrioritySettings lowPriority;
        private Camera mainCam;

        protected override void Awake()
        {
            base.Awake();
            highPriority = new PrioritySettings();
            lowPriority = new PrioritySettings();
            highPriority.Value = 100;
            lowPriority.Value = 0;
            mainCam = Camera.main;
            originalCullingMask = mainCam.cullingMask; // Store the original culling mask.
        }

        private void OnEnable()
        {
            SubscribeEvent<WhiteboardEntered>(OnWhiteboardEntered);
            SubscribeEvent<WhiteboardExited>(OnWhiteboardExited);
        }

        private void OnDisable()
        {
            UnsubscribeEvent<WhiteboardEntered>(OnWhiteboardEntered);
            UnsubscribeEvent<WhiteboardExited>(OnWhiteboardExited);
        }

        private void OnWhiteboardEntered(WhiteboardEntered monoEvent)
        {
            Debug.Log("Entered whiteboard");
            // Exclude the player layer for whiteboardCam by adjusting the main camera's culling mask directly using the playerLayer mask.
            mainCam.cullingMask &= ~playerLayer.value;
            freelookCam.Priority = lowPriority;
            monoEvent.WhiteboardCam.Priority = highPriority;
        }
        
        private void OnWhiteboardExited(WhiteboardExited monoEvent)
        {
            Debug.Log("Exited whiteboard");
            // Reset the culling mask to include the player layer again.
            mainCam.cullingMask = originalCullingMask;
            freelookCam.Priority = highPriority;
            monoEvent.WhiteboardCam.Priority = lowPriority;
        }
    }
}
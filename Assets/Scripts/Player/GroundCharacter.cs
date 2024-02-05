using UnityEngine;

namespace SparkGames.Portfolio3D.Player
{
    public class GroundCharacter : MonoBehaviour
    {
        [SerializeField] private float raycastDistance = 1.0f; 
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Vector3 characterGroundOffset;
        
        private readonly Vector3 rayOffset = Vector3.up * 0.3f;

        private void Update()
        {
            if (Physics.Raycast(transform.position + rayOffset, Vector3.down, out RaycastHit hit, raycastDistance, groundLayer))
            {
                transform.position = Vector3.Lerp(transform.position, hit.point + characterGroundOffset, 0.1f);
            }
        }
    }
}
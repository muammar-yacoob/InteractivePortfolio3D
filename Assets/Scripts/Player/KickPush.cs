using UnityEngine;

namespace SparkGames.Portfolio3D.Player
{
    public class KickPush : MonoBehaviour
    {
        [SerializeField] private float pushForce = 10f; 
        [SerializeField] private Transform kickPoint; 
        [SerializeField] private float kickRange = 1f; 
        [SerializeField] private LayerMask pushableLayer; 

        // This method is called by the animation event
        public void ApplyKickForce()
        {
            // Check for pushable objects around the kick point within the kick range
            Collider[] pushableObjects = Physics.OverlapSphere(kickPoint.position, kickRange, pushableLayer);
            foreach (var obj in pushableObjects)
            {
                if (obj.TryGetComponent(out Rigidbody rb))
                {
                    Vector3 direction = obj.transform.position - transform.position;
                    rb.AddForce(direction.normalized * pushForce, ForceMode.Impulse);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (kickPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(kickPoint.position, kickRange);
            }
        }
    }

}
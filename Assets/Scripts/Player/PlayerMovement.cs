using SparkCore.Runtime.Core;
using SparkCore.Runtime.Injection;
using UnityEngine;

namespace SparkGames.Portfolio3D.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : InjectableMonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float turnSpeed = 20f;
        [Inject] private IPlayerInput playerInput;

        private CharacterController characterController;
        private Transform cameraTransform;
        
        protected override void Awake()
        {
            base.Awake();
            characterController = GetComponent<CharacterController>();
            cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            var speed = playerInput.Movement.magnitude;
            var moveDirection = cameraTransform.forward * playerInput.Movement.y + cameraTransform.right * playerInput.Movement.x;
            moveDirection.y = 0;
            moveDirection.Normalize();
            moveDirection *= speed;
            

            // Rotate the character to face the move direction
            if (moveDirection != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
            }
            characterController.Move(moveDirection * (moveSpeed * Time.deltaTime));
        }
    }
}
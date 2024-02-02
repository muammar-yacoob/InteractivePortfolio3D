using SparkCore.Runtime.Core;
using SparkCore.Runtime.Injection;
using UnityEngine;

namespace SparkGames.Portfolio3D
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : InjectableMonoBehaviour
    {
        [Inject] private IPlayerInput playerInput;
        private Animator animator;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            var speed = playerInput.Movement.magnitude;
            animator.SetFloat("Speed", speed);
        }
    }
}
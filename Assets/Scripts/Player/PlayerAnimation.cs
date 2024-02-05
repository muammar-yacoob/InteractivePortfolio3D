using SparkCore.Runtime.Core;
using SparkCore.Runtime.Injection;
using UnityEngine;

namespace SparkGames.Portfolio3D.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : InjectableMonoBehaviour
    {
        [Inject] private IPlayerInput playerInput;
        private Animator animator;
        private int currentAnimationStateHash;
        private int idleStateHash;
        private int walkingStateHash;
        private const float speedThreshold = 0.1f;
        private const float crossFadeDuration = 0.03f;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();

            idleStateHash = Animator.StringToHash("Idle");
            walkingStateHash = Animator.StringToHash("Walking");
            currentAnimationStateHash = idleStateHash;
        }

        private void Update()
        {
            var speed = playerInput.Movement.magnitude;
            var newAnimationStateHash = speed > speedThreshold ? walkingStateHash : idleStateHash;

            if (currentAnimationStateHash != newAnimationStateHash)
            {
                animator.CrossFade(newAnimationStateHash, crossFadeDuration);
                currentAnimationStateHash = newAnimationStateHash;
            }
        }
    }
}
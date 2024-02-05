using Cysharp.Threading.Tasks;
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
        private int kickStateHash;
        private const float speedThreshold = 0.1f;
        private const float crossFadeDuration = 0.03f;

        private bool isKicking = false;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();

            idleStateHash = Animator.StringToHash("Idle");
            walkingStateHash = Animator.StringToHash("Walking");
            kickStateHash = Animator.StringToHash("Kick");
            currentAnimationStateHash = idleStateHash;

            playerInput.Kicked += OnKick; 
        }

        private void OnDestroy()
        {
            playerInput.Kicked -= OnKick;
        }

        private void Update()
        {
            if (isKicking) return;

            var speed = playerInput.Movement.magnitude;
            var newAnimationStateHash = speed > speedThreshold ? walkingStateHash : idleStateHash;

            if (currentAnimationStateHash != newAnimationStateHash)
            {
                animator.CrossFade(newAnimationStateHash, crossFadeDuration);
                currentAnimationStateHash = newAnimationStateHash;
            }
        }

        private void OnKick()
        {
            animator.CrossFade(kickStateHash, crossFadeDuration);
            isKicking = true; 
            currentAnimationStateHash = kickStateHash; 
            ResetKickState().Forget();
        }

        private async UniTask ResetKickState()
        {
            await UniTask.Delay(500);
            isKicking = false;
        }
    }
}

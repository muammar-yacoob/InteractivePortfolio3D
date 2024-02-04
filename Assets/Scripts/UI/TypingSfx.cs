using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SparkGames.Portfolio3D.UI
{
    [RequireComponent(typeof(AudioSource))]
    public class TypingSfx : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip typingSfx;
        [SerializeField] [Range(1, 10)] private int sfxFrequency = 1;

        private CancellationTokenSource cancellation;

        private void Awake()
        {
            audioSource ??= GetComponent<AudioSource>();
        }

        public async UniTask DOBeep(int totalCharacters, float durationPerChar, CancellationTokenSource cts)
        {
            cancellation = cts;
            if (audioSource == null || typingSfx == null) return;

            for (int i = 1; i <= totalCharacters; i++)
            {
                if (cancellation.Token.IsCancellationRequested) break;

                if (i % sfxFrequency == 0)
                {
                    audioSource.PlayOneShot(typingSfx);
                    // Calculate delay based on durationPerChar to synchronize with text animation.
                    float delay = durationPerChar * sfxFrequency * 1000; // Convert to milliseconds.
                    await UniTask.Delay((int)delay, cancellationToken: cancellation.Token);
                }
            }
        }
    }
}
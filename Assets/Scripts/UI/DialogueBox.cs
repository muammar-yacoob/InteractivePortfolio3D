using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using SparkCore.Runtime.Core;
using SparkGames.Portfolio3D.Stations;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace SparkGames.Portfolio3D.UI
{
    [RequireComponent(typeof(AudioSource))]
    public class DialogueBox : InjectableMonoBehaviour
    {
        [Header("FX Settings")]
        [SerializeField] private float moveDuration = 0.5f;
        [SerializeField] private float durationPerChar = 0.02f;
        [SerializeField] private AudioSource audioSource;
        
        [Header("UI Elements")]
        [SerializeField] private TMP_Text titleUI;
        [SerializeField] private TMP_Text textUI;
        [SerializeField] private Image iconUI;
        
        private RectTransform dialogueBox;
        private Vector2 offCanvasPosition;
        private Vector2 onCanvasPosition;
        private CancellationTokenSource textAnimationCts;
        private Vector2 position;
        private Material backgroundMat;
        private Color initialColor;
        private TypingSfx typingSfx;

        protected override void Awake()
        {
            base.Awake();
            dialogueBox ??= GetComponent<RectTransform>();
            audioSource ??= GetComponent<AudioSource>();

            position = dialogueBox.position;
            offCanvasPosition = new Vector2(position.x, -dialogueBox.rect.height*2);
            onCanvasPosition = position; // Current position as onCanvasPosition.
            
            // Initially move the dialogue box off canvas.
            position = offCanvasPosition;
            dialogueBox.position = position;
            
            backgroundMat = dialogueBox.GetComponent<Image>().material;
            initialColor = backgroundMat.color; 
            typingSfx = GetComponent<TypingSfx>()?? gameObject.AddComponent<TypingSfx>();
        }

        private void OnEnable()
        {
            SubscribeEvent<StationEntered>(OnStationEntered);
            SubscribeEvent<StationExited>(OnStationExited);
        }

        private void OnDisable()
        {
            UnsubscribeEvent<StationEntered>(OnStationEntered);
            UnsubscribeEvent<StationExited>(OnStationExited);
            backgroundMat.color = initialColor;
        }

        private async void OnStationEntered(StationEntered stationEntered)
        {
            // Cancel any ongoing text animation.
            textAnimationCts?.Cancel();
            textAnimationCts = new CancellationTokenSource();

            var token = stationEntered.Token;
            var stationInfo = stationEntered.StationInfo;

            if (token.IsCancellationRequested || textUI == null) return;
            
            titleUI.text = stationInfo.Title;
            backgroundMat.color = initialColor;
            iconUI.sprite = stationInfo.Icon;
            
            if(audioSource != null && stationInfo.EntrySfx != null)
            {
                audioSource.PlayOneShot(stationInfo.EntrySfx);
            }
            
            // Move dialogue box into view.
            await dialogueBox.DOMove(onCanvasPosition, moveDuration).AsyncWaitForCompletion();
            
            // Start text animation with new CancellationToken.
            try
            {
                var doTextTask = textUI.DOText(stationInfo.Dialogue, durationPerChar, true, textAnimationCts.Token);
                var doBeepTask = typingSfx.DOBeep(stationInfo.Dialogue.Length, durationPerChar, textAnimationCts);
                await UniTask.WhenAll(doTextTask, doBeepTask);
            }
            catch (OperationCanceledException)
            {
                OnStationExited(null);
            }
        }

        private async void OnStationExited(StationExited stationExited)
        {
            if (textAnimationCts == null || textUI == null) return;
            // Immediately cancel any ongoing text animation to prevent it from completing.
            textAnimationCts?.Cancel();

            // Move dialogue box out of view.
            var sequence = DOTween.Sequence();
            sequence.Append(dialogueBox.DOMove(offCanvasPosition, moveDuration));
            sequence.Join(backgroundMat.DOFade(0, moveDuration).SetEase(Ease.OutBack));
            
            await sequence.AsyncWaitForCompletion();
            textUI.text = string.Empty;
        }
    }
}

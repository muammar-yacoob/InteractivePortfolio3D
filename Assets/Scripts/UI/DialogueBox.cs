using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using SparkCore.Runtime.Core;
using SparkGames.Portfolio3D.Stations;
using System.Threading;
using UnityEngine.UI;

namespace SparkGames.Portfolio3D.UI
{
    public class DialogueBox : InjectableMonoBehaviour
    {
        [SerializeField] private float moveDuration = 0.5f;
        [SerializeField] private float durationPerChar = 0.02f;

        private TMP_Text dialogueText;
        private RectTransform dialogueBox;
        private Vector2 offCanvasPosition;
        private Vector2 onCanvasPosition;
        private CancellationTokenSource textAnimationCts;
        private Vector2 position;
        private Material mat;
        private Color initialColor;

        protected override void Awake()
        {
            base.Awake();
            dialogueBox ??= GetComponent<RectTransform>();
            dialogueText ??= GetComponentInChildren<TMP_Text>();

            position = dialogueBox.position;
            offCanvasPosition = new Vector2(position.x, -dialogueBox.rect.height*2);
            onCanvasPosition = position; // Current position as onCanvasPosition.
            
            // Initially move the dialogue box off canvas.
            position = offCanvasPosition;
            dialogueBox.position = position;
            
            mat = dialogueBox.GetComponent<Image>().material;
            initialColor = mat.color; 
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
            mat.color = initialColor;
        }

        private async void OnStationEntered(StationEntered stationEntered)
        {
            // Cancel any ongoing text animation.
            textAnimationCts?.Cancel();
            textAnimationCts = new CancellationTokenSource();

            if (stationEntered.Token.IsCancellationRequested || dialogueText == null) return;
            
            // Move dialogue box into view.
            mat.color = initialColor;
            await dialogueBox.DOMove(onCanvasPosition, moveDuration).AsyncWaitForCompletion();
            
            // Start text animation with new CancellationToken.
            try
            {
                await dialogueText.DOText(stationEntered.Dialogue, durationPerChar, true, textAnimationCts.Token);
            }
            catch (OperationCanceledException)
            {
                OnStationExited(null);
            }
        }

        private async void OnStationExited(StationExited stationExited)
        {
            if (textAnimationCts == null || dialogueText == null) return;
            // Immediately cancel any ongoing text animation to prevent it from completing.
            textAnimationCts?.Cancel();

            // Move dialogue box out of view.
            var sequence = DOTween.Sequence();
            sequence.Append(dialogueBox.DOMove(offCanvasPosition, moveDuration));
            sequence.Join(mat.DOFade(0, moveDuration).SetEase(Ease.OutBack));
            
            await sequence.AsyncWaitForCompletion();
            dialogueText.text = string.Empty;
            
        }
    }
}

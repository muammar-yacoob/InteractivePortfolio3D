﻿using System;
using System.Collections.Generic;
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
    [RequireComponent(typeof(CanvasGroup))]
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
        
        private Vector2 offCanvasPosition;
        private Vector2 onCanvasPosition;
        private CancellationTokenSource textAnimationCts;
        private Material backgroundMat;
        private Color initialColor;
        private TypingSfx typingSfx;
        private RectTransform dialogueBox;
        private CanvasGroup canvasGroup;
        
        private Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();
        private Dictionary<string, AudioClip> audioClipCache = new Dictionary<string, AudioClip>();


        protected override void Awake()
        {
            base.Awake();
            audioSource ??= GetComponent<AudioSource>();
            canvasGroup = GetComponent<CanvasGroup>();
            dialogueBox = GetComponent<RectTransform>();

            Canvas canvas = GetComponentInParent<Canvas>();
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            onCanvasPosition = dialogueBox.anchoredPosition; // Save the initial position
            offCanvasPosition = new Vector2(onCanvasPosition.x, -dialogueBox.rect.height);

            dialogueBox.anchoredPosition = offCanvasPosition;

            backgroundMat = dialogueBox.GetComponent<Image>().material;
            initialColor = backgroundMat.color;
            typingSfx = GetComponent<TypingSfx>() ?? gameObject.AddComponent<TypingSfx>();
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
            Debug.Log($"Station entered, loading icon: {stationEntered.ProjectData.Icon} and SFX: {stationEntered.ProjectData.SFX}");
            // Cancel any ongoing text animation.
            textAnimationCts?.Cancel();
            textAnimationCts = new CancellationTokenSource();

            var token = stationEntered.Token;
            var stationInfo = stationEntered.ProjectData;

            if (token.IsCancellationRequested || textUI == null) return;
    
            titleUI.text = stationInfo.Title;
            backgroundMat.color = initialColor;

            // Load and cache the icon sprite
            if (!spriteCache.TryGetValue(stationInfo.Icon, out var iconSprite))
            {
                iconSprite = Resources.Load<Sprite>("Icons/" + stationInfo.Icon);
                if (iconSprite != null)
                {
                    spriteCache[stationInfo.Icon] = iconSprite;
                }
            }
            iconUI.sprite = iconSprite;
    
            // Load and cache the audio clip
            if (!audioClipCache.TryGetValue(stationInfo.SFX, out var audioClip))
            {
                audioClip = Resources.Load<AudioClip>("SFX/" + stationInfo.SFX);
                if (audioClip != null)
                {
                    audioClipCache[stationInfo.SFX] = audioClip;
                }
            }

            if(audioSource != null && audioClip != null)
            {
                audioSource.PlayOneShot(audioClip);
            }

            
            // Set initial states
            canvasGroup.alpha = 0;
            dialogueBox.anchoredPosition = offCanvasPosition; // Make sure it starts off-screen.

            // Fade in the dialogue box along with its content
            canvasGroup.DOFade(1, moveDuration); // Fade in
            await dialogueBox.DOAnchorPos(onCanvasPosition, moveDuration).AsyncWaitForCompletion();

            // Start text animation with new CancellationToken.
            try
            {
                var doTextTask = textUI.DOText(stationInfo.Description, durationPerChar, true, textAnimationCts.Token);
                var doBeepTask = typingSfx.DOBeep(stationInfo.Description.Length, durationPerChar, textAnimationCts);
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
            textAnimationCts?.Cancel();

            // Move and fade out dialogue box
            canvasGroup.DOFade(0, moveDuration); // Fade out
            await dialogueBox.DOAnchorPos(offCanvasPosition, moveDuration).AsyncWaitForCompletion();

            // Clear texts after fade out
            textUI.text = string.Empty;
            titleUI.text = string.Empty;
        }
    }
}

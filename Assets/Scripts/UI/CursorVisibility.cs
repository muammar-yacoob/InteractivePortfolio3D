using SparkCore.Runtime.Core;
using SparkCore.Runtime.Injection;
using SparkGames.Portfolio3D.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SparkGames.Portfolio3D.UI
{
    public class CursorVisibility : InjectableMonoBehaviour
    {
        [Inject] private IPlayerInput playerInput;

        void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            playerInput.CursorVisibilityChanged += OnCursorVisibilityChanged;
        }

        void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame || Mouse.current.middleButton.wasPressedThisFrame)
            {
                HideCursor();
            }
        }

        private void OnCursorVisibilityChanged(bool isVisible)
        {
            Cursor.visible = isVisible;
            Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }

        private void HideCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDestroy()
        {
            if (playerInput != null)
            {
                playerInput.CursorVisibilityChanged -= OnCursorVisibilityChanged;
            }
        }
    }
}
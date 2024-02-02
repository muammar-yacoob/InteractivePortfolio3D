using System;
using SparkCore.Runtime.Injection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SparkGames.Portfolio3D
{
    [ServiceProvider]
    public class PlayerInput : IPlayerInput, IDisposable
    {
        private readonly PlayerControls playerControls;
        private Vector2 movement;
        public Vector2 Movement => movement;

        public PlayerInput()
        {
            playerControls = new PlayerControls();
            playerControls.GamePlay.Enable();
            playerControls.GamePlay.Movement.performed += MovementPerformed;
            playerControls.GamePlay.Movement.canceled += MovementCancelled;
        }
    
        private void MovementPerformed(InputAction.CallbackContext ctx) => movement = ctx.ReadValue<Vector2>();
        private void MovementCancelled(InputAction.CallbackContext ctx) => movement = Vector2.zero;

        public void Dispose()
        {
            playerControls.GamePlay.Disable();
            playerControls.GamePlay.Movement.performed -= MovementPerformed;
            playerControls.GamePlay.Movement.canceled -= MovementCancelled;
            playerControls?.Dispose();
        }
    }
}
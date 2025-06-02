using System;
using HGS.LastBastion.Core.Managers;
using UnityEngine;
using UnityEngine.InputSystem; // Required for new Input System

namespace HGS.LastBastion
{
    public class WeaponAim2D : MonoBehaviour
    {

        private enum AimControlType { None, Mouse, PrimaryMovement, Script }
        [SerializeField] private AimControlType aimControl;
        private InputManager inputManager;
        private Camera mainCamera;

        private void Awake()
        {
            inputManager = FindFirstObjectByType<InputManager>();
            mainCamera = Camera.main;

            InitializaAimControl();
        }

        private void InitializaAimControl()
        {
            switch (aimControl)
            {
                case AimControlType.Mouse:
                    inputManager.OnAimEvent += AimWeaponAtMouse;
                    break;
                case AimControlType.PrimaryMovement:
                    inputManager.OnMoveEvent += AimWeaponAtMovement;
                    break;
                case AimControlType.Script:
                    GetComponent<WeaponAutoAim2D>().Authorize = true;
                    break;
                default:
                    break;
            }
        }

        private void AimWeaponAtMovement(Vector2 movementDirection)
        {
            if (movementDirection == Vector2.zero)
                return;

            // Normalize the direction just in case
            Vector2 direction = movementDirection.normalized;

            // Calculate angle from movement direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Apply rotation
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            // Optional: Flip sprite based on aim direction
            Vector3 localScale = transform.localScale;
            if (angle > 90f || angle < -90f)
                localScale.y = -Mathf.Abs(localScale.y);
            else
                localScale.y = Mathf.Abs(localScale.y);
            transform.localScale = localScale;
        }

        private void AimWeaponAtMouse(Vector2 mouseScreenPosition)
        {
            // Get the mouse position from the input action
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
            mouseWorldPosition.z = 0f;

            // Calculate direction and angle
            Vector3 direction = mouseWorldPosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Apply rotation
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            // Optional: Flip sprite based on aim direction
            Vector3 localScale = transform.localScale;
            if (angle > 90f || angle < -90f)
                localScale.y = -Mathf.Abs(localScale.y);
            else
                localScale.y = Mathf.Abs(localScale.y);
            transform.localScale = localScale;
        }


    }
}

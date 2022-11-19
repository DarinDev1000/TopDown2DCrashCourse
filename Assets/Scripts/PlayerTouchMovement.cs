using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlayerTouchMovement : MonoBehaviour
{
    [SerializeField]
    private Vector2 JoystickSize = new(300, 300);
    [SerializeField]
    private FloatingJoystick Joystick; // This is the custom class I copied
    private PlayerController PlayerController;
    private Finger MovementFinger;
    private Finger AttackFinger;
    private Vector2 MovementAmount;

    private void OnEnable()
    {
        // Get the PlayerController instance currently attached to player
        PlayerController = gameObject.GetComponent<PlayerController>();

        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleFingerUp;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleFingerUp;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();
    }

    private void HandleFingerDown(Finger TouchedFinger)
    {
        // Handle Movement Finger
        if (MovementFinger == null && TouchedFinger.screenPosition.x <= Screen.width / 2f)
        {
            MovementFinger = TouchedFinger;
            MovementAmount = Vector2.zero;
            Joystick.gameObject.SetActive(true);
            Joystick.RectTransform.sizeDelta = JoystickSize;
            Joystick.RectTransform.anchoredPosition = ClampStartPosition(TouchedFinger.screenPosition);
        }

        // Handle Attack Finger
        if (AttackFinger == null && TouchedFinger.screenPosition.x > Screen.width / 2f)
        {
            AttackFinger = TouchedFinger;
            PlayerController.TrySwordAttack();
        }
    }

    private void HandleFingerUp(Finger LostFinger)
    {
        // Handle Movement Finger
        if (LostFinger == MovementFinger)
        {
            MovementFinger = null;
            Joystick.Knob.anchoredPosition = Vector2.zero;
            Joystick.gameObject.SetActive(false);
            MovementAmount = Vector2.zero;
        }

        // Handle Attack Finger
        if (LostFinger == AttackFinger)
        {
            AttackFinger = null;
        }
    }

    private void HandleFingerMove(Finger MovedFinger)
    {
        // Handle Movement Finger
        if (MovedFinger == MovementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = JoystickSize.x / 2f;
            ETouch.Touch currentTouch = MovedFinger.currentTouch;

            if (Vector2.Distance(
                currentTouch.screenPosition,
                Joystick.RectTransform.anchoredPosition
            ) > maxMovement)
            {
                knobPosition = (
                    currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition
                ).normalized
                * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition;
            }

            Joystick.Knob.anchoredPosition = knobPosition;
            MovementAmount = knobPosition / maxMovement;
        }

        // Handle Attack Finger
        // Don't need to currently handle attack finger moving
    }

    private Vector2 ClampStartPosition(Vector2 StartPosition)
    {
        // Clamp x position 
        if (StartPosition.x < JoystickSize.x / 2)
        {
            StartPosition.x = JoystickSize.x / 2;
        }
        else if (StartPosition.x > Screen.width - JoystickSize.x / 2)
        {
            StartPosition.x = Screen.width - JoystickSize.x / 2;
        }

        // Clamp y position 
        if (StartPosition.y < JoystickSize.y / 2)
        {
            StartPosition.y = JoystickSize.y / 2;
        }
        else if (StartPosition.y > Screen.height - JoystickSize.y / 2)
        {
            StartPosition.y = Screen.height - JoystickSize.y / 2;
        }

        return StartPosition;
    }

    private void Update()
    {
        // Move the player
        Vector2 scaledMovement = new(MovementAmount.x, MovementAmount.y);
        // print(scaledMovement);

        // Player.transform.LookAt(Player.transform.position + scaledMovement, Vector3.up);

        // Actually move the player
        if (MovementAmount.x > 0 || MovementAmount.y > 0)
        {
            PlayerController.SetPlayerMovement(scaledMovement);
        }
    }
}

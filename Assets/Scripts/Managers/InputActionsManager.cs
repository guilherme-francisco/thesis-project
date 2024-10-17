using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionsManager : MonoBehaviour
{
    public static InputActionsManager Instance { get; private set; }  
    public XRIDefaultInputActions InputActions { get; private set; }
    private void Awake() {
        Instance = this;

        InputActions = new XRIDefaultInputActions();

         // Left
		InputActions.XRILeftHandInteraction.Activate.performed += OnActivatePerformed;
        InputActions.XRILeftHandInteraction.PrimaryButton.performed += OnPrimaryButtonPerformed;
        InputActions.XRILeftHandInteraction.SecondaryButton.performed += OnSecondaryButtonPerformed;
        InputActions.XRILeftHandInteraction.MenuButton.performed += OnMenuButtonPerformed;

        // Right
        InputActions.XRIRightHandLocomotion.Move.performed += OnRightHandMovedPerformed;
    }

    private void OnMenuButtonPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Menu Left Hand performed!");
    }

    private void OnEnable()
    {
        // Left
        InputActions.XRILeftHandInteraction.PrimaryButton.Enable();
        InputActions.XRILeftHandInteraction.SecondaryButton.Enable();
        InputActions.XRILeftHandLocomotion.Move.Enable();
        InputActions.XRILeftHandInteraction.Activate.Enable();
        InputActions.XRILeftHandInteraction.Select.Enable();
        InputActions.XRILeftHandInteraction.MenuButton.Enable();
        // Right
        InputActions.XRIRightHandInteraction.PrimaryButton.Enable();
        InputActions.XRIRightHandInteraction.SecondaryButton.Enable();
        InputActions.XRIRightHandLocomotion.Move.Enable();
        InputActions.XRIRightHandInteraction.Select.Enable();
    }

    private void OnDisable()
    {
        // Left
        InputActions.XRILeftHandInteraction.PrimaryButton.Disable();
        InputActions.XRILeftHandInteraction.SecondaryButton.Disable();
        InputActions.XRILeftHandLocomotion.Move.Disable();
        InputActions.XRILeftHandInteraction.Activate.Disable();
        InputActions.XRILeftHandInteraction.Select.Disable();
        InputActions.XRILeftHandInteraction.MenuButton.Disable();
        // Right
        InputActions.XRIRightHandInteraction.PrimaryButton.Disable();
        InputActions.XRIRightHandInteraction.SecondaryButton.Disable();
        InputActions.XRIRightHandLocomotion.Move.Disable();
        InputActions.XRIRightHandInteraction.Select.Disable();
    }


    private void OnRightHandMovedPerformed(InputAction.CallbackContext context)
    {
        Vector2 moveDirection = context.ReadValue<Vector2>();
        Debug.Log("Right Hand Moved: " + moveDirection.ToString());
    }

    private void OnPrimaryButtonPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Primary Button performed!");
    }

    private void OnSecondaryButtonPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Secondary Button performed!");
    }

    private void OnActivatePerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Activate action performed on the left hand!");
    }

}

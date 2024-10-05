using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class SpherePrefab : MonoBehaviour
{
    public static SpherePrefab Instance { get; private set; }

    public event EventHandler<OnMeasurementEventArgs> OnMeasurementEvent;
    public class OnMeasurementEventArgs : EventArgs
    {
        public float measurementValue;
    }

    private void Awake() {
        Instance = this;
    }

    private void Start() {

        InputActionsManager.Instance.InputActions.XRILeftHand.MenuButton.performed += MenuButton_Performed;

        gameObject.SetActive(false);
    }

    private void MenuButton_Performed(InputAction.CallbackContext context)
    {
        if (context.interaction is HoldInteraction &&
            ToolsPanelUI.Instance.GetMode() == ToolsPanelUI.Modes.Measure &&
            MeasurementToolsUI.Instance.GetMeasurementTypes() == MeasurementToolsUI.MeasurementTypes.Volume &&
            MeasurementManager.Instance.GetCurrentVolumeMeasurementMethod() == MeasurementManager.VolumeMeasurementMethods.Prefab
        ) {
            OnMeasurementEvent?.Invoke(this, new OnMeasurementEventArgs {
                measurementValue = transform.localScale.x
            });
        }
    }

    private void Update() {
        XRIDefaultInputActions inputActions = InputActionsManager.Instance.InputActions;
        Vector2 thumbstickInput = inputActions.XRILeftHand.Move.ReadValue<Vector2>();

        transform.localScale += new Vector3(thumbstickInput.x * Time.deltaTime,
                                            thumbstickInput.x * Time.deltaTime, 
                                            thumbstickInput.x * Time.deltaTime);
    }

    private void ChangeLocalScaleWithLeftHand()
    {
        XRIDefaultInputActions inputActions = InputActionsManager.Instance.InputActions;
        Vector2 thumbstickInput = inputActions.XRILeftHand.Move.ReadValue<Vector2>();

        if (Mathf.Abs(thumbstickInput.x) > 0.5f)
        {
            float scroll = thumbstickInput.x;
            transform.localScale += new Vector3(0f, 0.1f, 0.1f);
        }
    }


    private void ChangeLocalScaleWithRighttHand()
    {
        XRIDefaultInputActions inputActions = InputActionsManager.Instance.InputActions;
        Vector2 thumbstickInput = inputActions.XRIRightHand.Move.ReadValue<Vector2>();

        if (Mathf.Abs(thumbstickInput.x) > 0.5f)
        {
            float scroll = thumbstickInput.x * Time.deltaTime;
            transform.localScale = new Vector3(0.01f, scroll, scroll);
        }
    }



}

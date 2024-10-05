using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CirclePrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI radiusValueText;
    [SerializeField] private TextMeshProUGUI areaValueText;
    [SerializeField] private TextMeshProUGUI diameterValueText;

    private XRRayInteractor rayInteractor;
    private const string RIGHT_CONTROLLER = "Right Controller";
    private const string LEFT_CONTROLLER = "Left Controller";

    private XRGrabInteractable grabInteractable;

    private void Start() {
        grabInteractable = GetComponent<XRGrabInteractable>();
        
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener((SelectExitEventArgs _) => {rayInteractor = null;});

        gameObject.SetActive(false);
    }


    private void Update() {
        XRIDefaultInputActions inputActions = InputActionsManager.Instance.InputActions;
        Vector2 thumbstickInput = inputActions.XRILeftHand.Move.ReadValue<Vector2>();

        transform.localScale += new Vector3(0f, thumbstickInput.x * Time.deltaTime, thumbstickInput.x * Time.deltaTime);

        diameterValueText.text = gameObject.transform.localScale.y.ToString("F2") + " m";
        radiusValueText.text = (gameObject.transform.localScale.y / 2).ToString("F2") + " m";
        areaValueText.text = (Math.Pow(gameObject.transform.localScale.y / 2, 2) * Math.PI).ToString("F2") + " m";
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


    private void OnGrabbed(SelectEnterEventArgs args)
    {
        XRBaseInteractor interactor = (XRBaseInteractor) args.interactorObject;

        if (interactor is XRRayInteractor)
        {
            rayInteractor = interactor as XRRayInteractor;
            
            string controllerName = rayInteractor.gameObject.name;
            Debug.Log("Object grabbed by: " + controllerName);
        }
    }
}

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System;

public class MeshClipper : MonoBehaviour
{
    public GameObject model3D;
    public Transform planeTransform;
    private Material cuttingMaterial;
    private const string RIGHT_CONTROLLER = "Right Controller";
    private const string LEFT_CONTROLLER = "Left Controller";

    private XRGrabInteractable grabInteractable;
    private XRRayInteractor rayInteractor;
    private ToolsPanelUI toolsPanelUI;
    [SerializeField] private float  rotateSpeed = 50f;


    private void Awake() {
        MeshRenderer renderer = model3D.GetComponent<MeshRenderer>();
        cuttingMaterial = renderer.material;
    }

    private void Start() {
        grabInteractable = planeTransform.GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener((SelectExitEventArgs _) => {rayInteractor = null;});

        planeTransform.gameObject.SetActive(false);
        toolsPanelUI = ToolsPanelUI.Instance;

    }
    private void RotatePlaneWithLeftHand()
    {
        XRIDefaultInputActions inputActions = InputActionsManager.Instance.InputActions;
        Vector2 thumbstickInput = inputActions.XRILeftHand.Move.ReadValue<Vector2>();

        Debug.Log("MeshClipper Left input: " + thumbstickInput.ToString());

        if (inputActions.XRILeftHand.PrimaryButton.IsPressed())
        {
            planeTransform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
        }

        if (inputActions.XRILeftHand.SecondaryButton.IsPressed())
        {
            planeTransform.Rotate(Vector3.back, rotateSpeed * Time.deltaTime);
        }

        if (Mathf.Abs(thumbstickInput.x) > 0.5f)
        {
            float rotationAmount = Mathf.Sign(thumbstickInput.x) * rotateSpeed * Time.deltaTime;
            planeTransform.Rotate(Vector3.up, rotationAmount);
        }

        if (Mathf.Abs(thumbstickInput.y) > 0.5f)
        {
            float rotationAmount = Mathf.Sign(thumbstickInput.y) * rotateSpeed * Time.deltaTime;
            planeTransform.Rotate(Vector3.right, rotationAmount);
        }
    }

    private void RotatePlaneWithRightHand()
    {
        XRIDefaultInputActions inputActions = InputActionsManager.Instance.InputActions;
        Vector2 thumbstickInput = inputActions.XRIRightHand.Move.ReadValue<Vector2>();

        Debug.Log("MeshClipper Right input: " + thumbstickInput.ToString());

        if (inputActions.XRIRightHand.PrimaryButton.IsPressed())
        {
            planeTransform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
        }

        if (inputActions.XRIRightHand.SecondaryButton.IsPressed())
        {
            planeTransform.Rotate(Vector3.back, rotateSpeed * Time.deltaTime);
        }

        if (Mathf.Abs(thumbstickInput.x) > 0.5f)
        {
            float rotationAmount = Mathf.Sign(thumbstickInput.x) * rotateSpeed * Time.deltaTime;
            planeTransform.Rotate(Vector3.up, rotationAmount);
        }

        if (Mathf.Abs(thumbstickInput.y) > 0.5f)
        {
            float rotationAmount = Mathf.Sign(thumbstickInput.y) * rotateSpeed * Time.deltaTime;
            planeTransform.Rotate(Vector3.right, rotationAmount);
        }
    }


    void Update()
    {
        //TODO: Create an event for when the modes changes 
        if (toolsPanelUI.GetMode() == ToolsPanelUI.Modes.Clip) {
            planeTransform.gameObject.SetActive(true);

            if (rayInteractor != null) {
                if (rayInteractor.gameObject.name == RIGHT_CONTROLLER) 
                RotatePlaneWithLeftHand();
                else if (rayInteractor.gameObject.name == LEFT_CONTROLLER) 
                RotatePlaneWithRightHand();
            }

            Vector3 planePosition = planeTransform.position;
            Vector3 planeNormal = planeTransform.up * -1;

            cuttingMaterial.SetVector("_PlanePosition", planePosition);
            cuttingMaterial.SetVector("_PlaneNormal", planeNormal);
        }  else {
            planeTransform.gameObject.SetActive(false);
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

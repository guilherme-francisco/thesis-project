// transforms an object: 
// - hold and drag primary mouse button to rotate the cube around its centre
// - hold and drag primary mouse button to rotate the cube around the camera (useful when inside the cube)
// - use arrow keys to translate the cube left/right and forward/backward
// - scroll wheel to scale the cube up/down
// adapted from various online sources
// Gilles Ferrand, University of Manitoba 2016

using System;
using System.Collections.Generic;


// transforms an object: 
// - hold and drag primary mouse button to rotate the cube around its centre
// - hold and drag primary mouse button to rotate the cube around the camera (useful when inside the cube)
// - use arrow keys to translate the cube left/right and forward/backward
// - scroll wheel to scale the cube up/down
// adapted from various online sources
// Gilles Ferrand, University of Manitoba 2016

using UnityEngine;
using UnityEngine.InputSystem;
using EzySlice;
using UnityEngine.InputSystem.HID;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    // Input action events
    public event EventHandler OnActivateEvent;
    public event EventHandler OnSelectEvent;
    public event EventHandler<OnRightHandMoveArgs> OnRightHandMove;

    public class OnRightHandMoveArgs : EventArgs
    {
        public Vector2 moveDirection;
    }

    // Measurement

    public event EventHandler<OnMeasurementEventArgs> OnMeasurementEvent;
    public class OnMeasurementEventArgs : EventArgs
    {
        public float measurementValue;
    }

    [Header("3D Model")]
    [SerializeField] private GameObject model3D;

    [Header("Tag")]
    [SerializeField] private float sphereRadius = 1.0f;
    [SerializeField] private GameObject spherePrefab;


    private List<GameObject> spheres = new();

    [Header("Rotate")]
    [SerializeField] private float  rotateSpeed = 4.0f;

    [SerializeField] private float zoomSpeed = 1.0f;

    [Header("Translate")]
    [SerializeField] private float panSpeed = 2.0f;


    [Header("Scale")]
    [SerializeField] private float scaleMin = 0.01f;
	[SerializeField] private float scaleMax = 100f;

    [Header("Clipping")]
    private Material volumetricMaterial;
    [SerializeField] Transform clippingPlane;

    private bool isClipped = false;

    
    [Header("Navigation")]
    [SerializeField] private Transform xrOrigin;

    [Header("Main Controller")]
    [SerializeField] private GameObject leftController;


    private XRIDefaultInputActions inputActions;
	private ToolsPanelUI toolsPanelUI;


	void Awake () {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Duplicate instance of GameManager. Destroying the duplicate.");
            Destroy(gameObject);
        }

        inputActions = new XRIDefaultInputActions();

        // Left
		inputActions.XRILeftHand.Activate.performed += OnActivatePerformed;
		inputActions.XRILeftHand.Select.performed += OnSelectPerformed;
        inputActions.XRILeftHand.PrimaryButton.performed += OnPrimaryButtonPerformed;
        inputActions.XRILeftHand.SecondaryButton.performed += OnSecondaryButtonPerformed;
        // Right
        inputActions.XRIRightHand.Move.performed += OnRightHandMovedPerformed;
	}

    private void OnRightHandMovedPerformed(InputAction.CallbackContext context)
    {
        Vector2 moveDirection = context.ReadValue<Vector2>();
        Debug.Log("Right Hand Moved: " + moveDirection.ToString());

        OnRightHandMove?.Invoke(this, new OnRightHandMoveArgs
        {
            moveDirection = moveDirection,
        });
    }

    private void OnPrimaryButtonPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Primary Button performed!");
    }

    private void OnSecondaryButtonPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Secondary Button performed!");
    }


    void Start () {
        toolsPanelUI = ToolsPanelUI.Instance;
        volumetricMaterial = model3D.GetComponentInChildren<Renderer>().material;
        toolsPanelUI.onNavigateEvent += ToolsPanelUI_OnNavigateEvent;
    }

    private void ToolsPanelUI_OnNavigateEvent(object sender, EventArgs e)
    {
        xrOrigin.localScale /= 10;
        xrOrigin.position = model3D.transform.position;
    }

    void Update()
    {
        switch (toolsPanelUI.GetMode())
        {
            case ToolsPanelUI.Modes.Rotate:
                RotateModel();
                break;
            case ToolsPanelUI.Modes.Move:
                MoveModel();
                break;
            case ToolsPanelUI.Modes.Scale:
                ScaleModel();
                break;
            case ToolsPanelUI.Modes.Tag:
                HandleTag();
                break;
            case ToolsPanelUI.Modes.Clip:
                HandleClip();
                break;
        }

    }

    private void HandleClip()
    {
        SlicedHull hull = model3D.Slice(clippingPlane.position, clippingPlane.up);

        if (hull != null && !isClipped == true) {
            Debug.Log("Model Clip successfully");
            hull.CreateLowerHull(model3D);
            model3D.SetActive(false);
            isClipped = true; 
        }
    }

    private void OnSelectPerformed(InputAction.CallbackContext context)
    {

        Debug.Log("Select action performed on the left hand!");

        switch (toolsPanelUI.GetMode())
        {
            case ToolsPanelUI.Modes.Measure:
                HandleMeasure();
                break;
        }

        OnSelectEvent?.Invoke(this, EventArgs.Empty);
    }

    private void OnEnable()
    {
        // Left
        inputActions.XRILeftHand.PrimaryButton.Enable();
        inputActions.XRILeftHand.SecondaryButton.Enable();
        inputActions.XRILeftHand.Move.Enable();
        inputActions.XRILeftHand.Activate.Enable();
        inputActions.XRILeftHand.Select.Enable();
        // Right
        inputActions.XRIRightHand.PrimaryButton.Enable();
        inputActions.XRIRightHand.SecondaryButton.Enable();
        inputActions.XRIRightHand.Move.Enable();
    }

    private void OnDisable()
    {
        // Left
        inputActions.XRILeftHand.PrimaryButton.Disable();
        inputActions.XRILeftHand.SecondaryButton.Disable();
        inputActions.XRILeftHand.Move.Disable();
        inputActions.XRILeftHand.Activate.Disable();
        inputActions.XRILeftHand.Select.Disable();
        // Right
        inputActions.XRIRightHand.PrimaryButton.Disable();
        inputActions.XRIRightHand.SecondaryButton.Disable();
        inputActions.XRIRightHand.Move.Disable();
    }

    private void OnActivatePerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Activate action performed on the left hand!");

        OnActivateEvent?.Invoke(this, EventArgs.Empty);
    }

    // Move 
    private void MoveModel()
    {
        Vector2 thumbstickInput = inputActions.XRILeftHand.Move.ReadValue<Vector2>();

        Debug.Log("Move vector:" + thumbstickInput.ToString());

        if (inputActions.XRILeftHand.PrimaryButton.IsPressed())
        {
            Vector3 moveDirection = Camera.main.transform.up;
            float distance = panSpeed * Time.deltaTime;
            model3D.transform.Translate(moveDirection * distance, Space.World);
        }

        if (inputActions.XRILeftHand.SecondaryButton.IsPressed())
        {
            Vector3 moveDirection = -Camera.main.transform.up;
            float distance = panSpeed * Time.deltaTime;
            model3D.transform.Translate(moveDirection * distance, Space.World);
        }

        if (Mathf.Abs(thumbstickInput.x) > 0.5f)
        {
            float direction = Mathf.Sign(thumbstickInput.x);
            Vector3 translation = direction * Camera.main.transform.right * panSpeed * Time.deltaTime;
            model3D.transform.Translate(translation, Space.World);
        }

        if (Mathf.Abs(thumbstickInput.y) > 0.5f)
        {
            float direction = Mathf.Sign(thumbstickInput.y);
            Vector3 translation = direction * Camera.main.transform.forward * panSpeed * Time.deltaTime;
            model3D.transform.Translate(translation, Space.World);
        }
    }

    // Rotate

    private void RotateModel()
    {
        Vector2 thumbstickInput = inputActions.XRILeftHand.Move.ReadValue<Vector2>();

        Debug.Log("Thumbstick input: " + thumbstickInput.ToString());

        if (inputActions.XRILeftHand.PrimaryButton.IsPressed())
        {
            model3D.transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
        }

        if (inputActions.XRILeftHand.SecondaryButton.IsPressed())
        {
            model3D.transform.Rotate(Vector3.back, rotateSpeed * Time.deltaTime);
        }

        if (Mathf.Abs(thumbstickInput.x) > 0.5f)
        {
            float rotationAmount = Mathf.Sign(thumbstickInput.x) * rotateSpeed * Time.deltaTime;
            model3D.transform.Rotate(Vector3.up, rotationAmount);
        }

        if (Mathf.Abs(thumbstickInput.y) > 0.5f)
        {
            float rotationAmount = Mathf.Sign(thumbstickInput.y) * rotateSpeed * Time.deltaTime;
            model3D.transform.Rotate(Vector3.right, rotationAmount);
        }
    }


    // Scale 

    private void ScaleModel()
    {
        Vector2 thumbstickInput = inputActions.XRILeftHand.Move.ReadValue<Vector2>();

        float scale = Mathf.Clamp(model3D.transform.localScale[0], scaleMin, scaleMax);

        float scroll = thumbstickInput.y * Time.deltaTime;
        
        Debug.Log("Scale Factor:" + scroll.ToString());
        
        bool isScaling = Mathf.Abs(scroll) > 0;

        if (isScaling)
        {
            scale *= 1 + scroll * zoomSpeed;
            scale = Mathf.Clamp(scale, scaleMin, scaleMax);
            model3D.transform.localScale = new Vector3(scale, scale, scale);
            if (Mathf.Approximately(scroll, 0)) isScaling = false;
        }
    }

    // Tag
    private bool TryToCreateSphere(out GameObject sphere)
    {
        RaycastHit hit;

        if (Physics.Raycast(leftController.transform.position,
            leftController.transform.forward, out hit))
        {
            if (!hit.collider.CompareTag("SpherePrefab"))
            {
                sphere = Instantiate(spherePrefab, hit.point, Quaternion.identity);
                sphere.transform.localScale = Vector3.one * sphereRadius * 2f;

                return true;
            }
        }

        sphere = null;
        return false;
    }

    private void HandleTag()
    {
        if (inputActions.XRILeftHand.PrimaryButton.triggered)
        {
            bool success = TryToCreateSphere(out GameObject sphere);

            if (success)
            {
                Debug.Log("Sphere created successfully!");
            }
            else
            {
                // Sphere creation failed
                Debug.Log("Failed to create sphere.");
            }
        }
        else if (inputActions.XRILeftHand.SecondaryButton.triggered)
        {
            RaycastHit hit;
            if (Physics.Raycast(leftController.transform.position,
            leftController.transform.forward, out hit))
            {
                if (hit.collider.CompareTag("SpherePrefab"))
                {
                    Destroy(hit.collider.gameObject);
                }
            }

        }
    }

    // Measure

    private void HandleMeasure()
    {
        if (spheres.Count >= 2)
        {
            foreach (GameObject sphere in spheres)
            {
                Destroy(sphere);
            }
            spheres.Clear();
        }

        if (TryToCreateSphere(out GameObject newSphere))
        {
            spheres.Add(newSphere);
        }

        if (spheres.Count == 2)
        {
            float distance = Vector3.Distance(spheres[0].transform.position, spheres[1].transform.position);
            Debug.Log("Distance between spheres: " + distance);

            OnMeasurementEvent?.Invoke(this, new OnMeasurementEventArgs
            {
                measurementValue = distance
            });
        }
    }

    // Clipping 
    public void OnXSliderValueChanged(float value)
    {
        Debug.Log("XSlider changed: " + value);
        volumetricMaterial.SetFloat("_SliceAxis1Min", value);
        //volumetricMaterial.SetFloat("_SliceAxis1Max", 1 - value);
    }

    public void OnYSliderValueChanged(float value)
    {
        Debug.Log("YSlider changed: " + value);
        volumetricMaterial.SetFloat("_SliceAxis2Min", value);
        //volumetricMaterial.SetFloat("_SliceAxis2Max", 1 - value);
    }

    public void OnZSliderValueChanged(float value)
    {
        Debug.Log("ZSlider changed: " + value);
        volumetricMaterial.SetFloat("_SliceAxis3Min", value);
        //volumetricMaterial.SetFloat("_SliceAxis3Max", 1 - value);
    }
}

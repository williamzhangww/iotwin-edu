using UnityEngine;

public class DesktopPowerSwitch : MonoBehaviour
{
    [Header("Button Settings")]
    public Renderer buttonRenderer;
    public Color offColor = Color.red;
    public Color onColor = Color.green;
    public Transform buttonTransform;
    public Vector3 pressOffset = new Vector3(0, -0.005f, 0);

    [Header("Display Objects")]
    public LineRenderer laserLine;      // Controls only the laser
    public GameObject canvasLCD;
    public GameObject distanceValueObj;

    [Header("Sound Effects")]
    public AudioSource audioSource;

    [Header("Power State")]
    public bool powerOn = false;

    private Vector3 initialPos;
    private Camera mainCamera;

    void Start()
    {
        if (buttonTransform != null)
            initialPos = buttonTransform.localPosition;

        mainCamera = Camera.main;

        UpdateUI();
    }

    void Update()
    {
        // Check if raycast hits the button
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // Mouse hover ¡ú show hand cursor
                MouseCursorManager.SetHandCursor();

                if (Input.GetMouseButtonDown(0))
                {
                    TogglePower();
                }
                return; // Exit if hovering, do not reset cursor
            }
        }

        // Not hitting the button ¡ú reset mouse cursor
        MouseCursorManager.ResetCursor();
    }

    void TogglePower()
    {
        powerOn = !powerOn;

        // Button animation
        if (buttonTransform != null)
            buttonTransform.localPosition = initialPos + (powerOn ? pressOffset : Vector3.zero);

        // Change button color
        if (buttonRenderer != null)
            buttonRenderer.material.color = powerOn ? onColor : offColor;

        // Activate/deactivate UI: only hide the laser, not the entire DT50
        if (laserLine != null) laserLine.enabled = powerOn;
        if (canvasLCD != null) canvasLCD.SetActive(powerOn);
        if (distanceValueObj != null) distanceValueObj.SetActive(powerOn);

        // Play sound effect
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.Play();
        }
    }

    void UpdateUI()
    {
        if (buttonRenderer != null)
            buttonRenderer.material.color = powerOn ? onColor : offColor;

        if (laserLine != null) laserLine.enabled = powerOn;
        if (canvasLCD != null) canvasLCD.SetActive(powerOn);
        if (distanceValueObj != null) distanceValueObj.SetActive(powerOn);
    }
}

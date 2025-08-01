using UnityEngine;

public class SensorLightControllerdesktop : MonoBehaviour
{
    [Header("References")]
    public DesktopPowerSwitch powerSwitch; // Drag your power switch object here
    public Renderer sensorRenderer;        // The mesh with the lights material

    [Header("Colors")]
    public Color offColor = Color.gray;    // Light OFF color
    public Color onColor = Color.yellow;   // Light ON color

    private int metalCount = 0;            // How many metal objects are inside the trigger

    void Start()
    {
        if (sensorRenderer != null)
            sensorRenderer.material.color = offColor;
    }

    void Update()
    {
        UpdateLightState();
    }

    private void UpdateLightState()
    {
        // Lights ON only if power switch is ON AND at least one metal object is inside
        if (powerSwitch != null && powerSwitch.powerOn && metalCount > 0)
        {
            sensorRenderer.material.color = onColor;
        }
        else
        {
            sensorRenderer.material.color = offColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object has the tag "Metal"
        if (other.CompareTag("Metal"))
        {
            metalCount++;
            UpdateLightState();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Metal"))
        {
            metalCount--;
            if (metalCount < 0) metalCount = 0; // Safety check
            UpdateLightState();
        }
    }
}

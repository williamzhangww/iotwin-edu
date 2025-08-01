using UnityEngine;

public class IME12Trigger : MonoBehaviour
{
    [Header("LED Control")]
    public MeshRenderer ledRenderer;   // Drag the MeshRenderer of IME12_LED here
    public Material offMaterial;       // Material when LED is off
    public Material onMaterial;        // Material when LED is on

    [Header("Power Switch Reference")]
    public PowerSwitchSmooth powerSwitch;   // Drag the object with the PowerSwitchSmooth script here

    private bool isInside = false;

    private void OnTriggerEnter(Collider other)
    {
        // Do not respond if the power is off
        if (powerSwitch != null && !powerSwitch.IsPowerOn())
        {
            SetLedOff();
            return;
        }

        if (other.CompareTag("Metal"))
        {
            isInside = true;
            SetLedOn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Metal"))
        {
            isInside = false;
            SetLedOff();
        }
    }

    private void Update()
    {
        // If power is off, turn off the LED directly
        if (powerSwitch != null && !powerSwitch.IsPowerOn())
        {
            SetLedOff();
            return;
        }

        // If power is on but nothing is inside the trigger, ensure the LED is off
        if (!isInside)
        {
            SetLedOff();
        }
    }

    private void SetLedOn()
    {
        if (ledRenderer != null && onMaterial != null)
            ledRenderer.material = onMaterial;
    }

    private void SetLedOff()
    {
        if (ledRenderer != null && offMaterial != null)
            ledRenderer.material = offMaterial;
    }
}

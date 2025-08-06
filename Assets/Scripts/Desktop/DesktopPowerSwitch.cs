using UnityEngine;
using System.Collections;

public class DesktopPowerSwitch : MonoBehaviour, IMouseHoverInteractable
{
    [Header("Button Settings")]
    public Renderer buttonRenderer;
    public Color offColor = Color.red;
    public Color onColor = Color.green;
    public Transform buttonTransform;
    public Vector3 pressOffset = new Vector3(0, -0.005f, 0);

    [Header("Display Objects")]
    public LineRenderer laserLine;
    public GameObject canvasLCD;
    public GameObject distanceValueObj;

    [Header("Sound Effects")]
    public AudioSource audioSource;

    [Header("Power State")]
    public bool powerOn = false;

    [Header("Hint Text")]
    public GameObject hintTextObject;
    public float hintHideDelay = 3f;

    private Vector3 initialPos;
    private bool hasStartedHide = false;

    void Start()
    {
        if (buttonTransform != null)
            initialPos = buttonTransform.localPosition;

        if (hintTextObject != null)
            hintTextObject.SetActive(true);

        UpdateUI();
    }

    public void OnMouseHoverEnter() => MouseCursorManager.SetHandCursor();
    public void OnMouseHoverExit() => MouseCursorManager.ResetCursor();

    public void OnMouseClick()
    {
        TogglePower();

        if (!hasStartedHide && hintTextObject != null)
        {
            hasStartedHide = true;
            StartCoroutine(HideHintAfterDelay());
        }
    }

    void TogglePower()
    {
        powerOn = !powerOn;

        if (buttonTransform != null)
            buttonTransform.localPosition = initialPos + (powerOn ? pressOffset : Vector3.zero);

        if (buttonRenderer != null)
            buttonRenderer.material.color = powerOn ? onColor : offColor;

        if (laserLine != null) laserLine.enabled = powerOn;
        if (canvasLCD != null) canvasLCD.SetActive(powerOn);
        if (distanceValueObj != null) distanceValueObj.SetActive(powerOn);

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

    IEnumerator HideHintAfterDelay()
    {
        yield return new WaitForSeconds(hintHideDelay);
        if (hintTextObject != null)
            hintTextObject.SetActive(false);
    }
}
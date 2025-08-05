using UnityEngine;
using TMPro;
using System.Collections;

public class DesktopPT100Heater : MonoBehaviour
{
    [Header("UI Display")]
    public TextMeshProUGUI tempText;
    public TextMeshProUGUI currentText;
    public TextMeshProUGUI voltageText;

    [Header("Temperature Settings")]
    public float startTemp = 23f;
    public float targetTemp = 32f;
    public float heatDuration = 12f; // seconds

    [Header("Mouse Settings")]
    public Camera mainCamera;
    public Texture2D handCursor;
    [Tooltip("Hotspot offset (x,y) to align the hand cursor with raycast hit point")]
    public Vector2 hotspotOffset = new Vector2(300, 300);

    [Header("Hint Text")]
    public GameObject hintTextObject;
    public float hintHideDelay = 3f;

    private float currentTemp;
    private bool heating = false;
    private bool cursorOverPT100 = false;
    private bool hasStartedHintHide = false;

    void Start()
    {
        currentTemp = startTemp;
        UpdateDisplays(currentTemp);

        if (mainCamera == null)
            mainCamera = Camera.main;

        if (hintTextObject != null)
            hintTextObject.SetActive(true);
    }

    void Update()
    {
        cursorOverPT100 = false;

        // Raycast to check if cursor is over the PT100 object
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                cursorOverPT100 = true;

                // Hold left mouse button to heat
                if (Input.GetMouseButton(0))
                {
                    heating = true;

                    // Start countdown to hide hint on first activation
                    if (!hasStartedHintHide)
                    {
                        hasStartedHintHide = true;
                        StartCoroutine(HideHintAfterDelay());
                    }
                }
                else
                {
                    heating = false;
                }
            }
        }

        // Automatically stop heating if not pointing at PT100
        if (!cursorOverPT100)
            heating = false;

        // Update cursor appearance
        if (cursorOverPT100)
        {
            MouseCursorManager.SetHandCursor();
        }
        else
        {
            MouseCursorManager.ResetCursor();
        }

        // Smooth temperature update
        float target = heating ? targetTemp : startTemp;
        float rate = Mathf.Abs(targetTemp - startTemp) / heatDuration;
        currentTemp = Mathf.MoveTowards(currentTemp, target, rate * Time.deltaTime);

        UpdateDisplays(currentTemp);
    }

    void UpdateDisplays(float temp)
    {
        if (tempText != null)
            tempText.text = temp.ToString("F1");

        // Current mapping based on temperature
        float current = 8.40f + (temp - 24.5f) * (9.15f - 8.40f) / (33.0f - 24.5f);
        if (currentText != null)
            currentText.text = current.ToString("F2");

        // Voltage mapping based on temperature
        float voltage = 2.50f + (temp - 25.0f) * (2.84f - 2.50f) / (28.4f - 25.0f);
        if (voltageText != null)
            voltageText.text = voltage.ToString("F2");
    }

    private IEnumerator HideHintAfterDelay()
    {
        yield return new WaitForSeconds(hintHideDelay);
        if (hintTextObject != null)
        {
            hintTextObject.SetActive(false);
        }
    }
}

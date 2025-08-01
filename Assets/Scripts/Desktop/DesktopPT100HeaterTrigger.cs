using UnityEngine;
using TMPro;

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
    [Tooltip("Hotspot offset (x,y) to align hand cursor with mouse raycast position")]
    public Vector2 hotspotOffset = new Vector2(300, 300); // Hotspot offset

    private float currentTemp;
    private bool heating = false;
    private bool cursorOverPT100 = false;

    void Start()
    {
        currentTemp = startTemp;
        UpdateDisplays(currentTemp);

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        cursorOverPT100 = false;

        // Mouse raycast check: is the PT100 being pointed at?
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                cursorOverPT100 = true;

                // Hold down left mouse button -> start heating
                if (Input.GetMouseButton(0))
                {
                    heating = true;
                }
                else
                {
                    heating = false;
                }
            }
        }

        // When not pointing at PT100, automatically stop heating
        if (!cursorOverPT100)
            heating = false;

        // Update mouse cursor
        if (cursorOverPT100)
        {
            MouseCursorManager.SetHandCursor();

            if (Input.GetMouseButton(0))
            {
                heating = true;
            }
        }
        else
        {
            MouseCursorManager.ResetCursor();
        }

        // Smoothly increase or decrease temperature
        float target = heating ? targetTemp : startTemp;
        float rate = Mathf.Abs(targetTemp - startTemp) / heatDuration;
        currentTemp = Mathf.MoveTowards(currentTemp, target, rate * Time.deltaTime);

        UpdateDisplays(currentTemp);
    }

    void UpdateDisplays(float temp)
    {
        if (tempText != null)
            tempText.text = temp.ToString("F1");

        // Current - temperature mapping
        float current = 8.40f + (temp - 24.5f) * (9.15f - 8.40f) / (33.0f - 24.5f);
        if (currentText != null)
            currentText.text = current.ToString("F2");

        // Voltage - temperature mapping
        float voltage = 2.50f + (temp - 25.0f) * (2.84f - 2.50f) / (28.4f - 25.0f);
        if (voltageText != null)
            voltageText.text = voltage.ToString("F2");
    }
}

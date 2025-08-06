using UnityEngine;
using TMPro;
using System.Collections;

public class DesktopPT100Heater : MonoBehaviour, IMouseHoverInteractable
{
    [Header("UI Display")]
    public TextMeshProUGUI tempText;
    public TextMeshProUGUI currentText;
    public TextMeshProUGUI voltageText;

    [Header("Temperature Settings")]
    public float startTemp = 23f;
    public float targetTemp = 32f;
    public float heatDuration = 12f;

    [Header("Hint Text")]
    public GameObject hintTextObject;
    public float hintHideDelay = 3f;

    private float currentTemp;
    private bool heating = false;
    private bool hasStartedHintHide = false;

    void Start()
    {
        currentTemp = startTemp;
        UpdateDisplays(currentTemp);

        if (hintTextObject != null)
            hintTextObject.SetActive(true);
    }

    void Update()
    {
        float target = heating ? targetTemp : startTemp;
        float rate = Mathf.Abs(targetTemp - startTemp) / heatDuration;
        currentTemp = Mathf.MoveTowards(currentTemp, target, rate * Time.deltaTime);

        UpdateDisplays(currentTemp);
    }

    public void OnMouseHoverEnter() => MouseCursorManager.SetHandCursor();
    public void OnMouseHoverExit()
    {
        MouseCursorManager.ResetCursor();
        heating = false;
    }

    public void OnMouseClick()
    {
        heating = true;

        if (!hasStartedHintHide)
        {
            hasStartedHintHide = true;
            StartCoroutine(HideHintAfterDelay());
        }
    }

    void UpdateDisplays(float temp)
    {
        if (tempText != null)
            tempText.text = temp.ToString("F1");

        float current = 8.40f + (temp - 24.5f) * (9.15f - 8.40f) / (33.0f - 24.5f);
        if (currentText != null)
            currentText.text = current.ToString("F2");

        float voltage = 2.50f + (temp - 25.0f) * (2.84f - 2.50f) / (28.4f - 25.0f);
        if (voltageText != null)
            voltageText.text = voltage.ToString("F2");
    }

    IEnumerator HideHintAfterDelay()
    {
        yield return new WaitForSeconds(hintHideDelay);
        if (hintTextObject != null)
            hintTextObject.SetActive(false);
    }
}
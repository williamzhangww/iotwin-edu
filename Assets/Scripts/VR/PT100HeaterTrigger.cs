using UnityEngine;
using TMPro;

public class TempHeater : MonoBehaviour
{
    [Header("UI Display")]
    public TextMeshProUGUI tempText;
    public TextMeshProUGUI currentText;
    public TextMeshProUGUI voltageText;

    [Header("Temperature Settings")]
    public float startTemp = 23f;
    public float targetTemp = 32f;
    public float heatDuration = 12f; // second
    public string handTag = "Hand";

    // Current - Temperature mapping
    private const float refTemp1 = 24.5f;
    private const float refCurrent1 = 8.40f;
    private const float refTemp2 = 33.0f;
    private const float refCurrent2 = 9.15f;

    // Voltage - Temperature mapping
    private const float refVoltageTemp1 = 25.0f;
    private const float refVoltage1 = 2.50f;
    private const float refVoltageTemp2 = 28.4f;
    private const float refVoltage2 = 2.84f;

    private float currentTemp;
    private bool heating = false;

    void Start()
    {
        currentTemp = startTemp;
        UpdateDisplays(currentTemp);
    }

    void Update()
    {
        float target = heating ? targetTemp : startTemp;
        float rate = Mathf.Abs(targetTemp - startTemp) / heatDuration;
        currentTemp = Mathf.MoveTowards(currentTemp, target, rate * Time.deltaTime);
        UpdateDisplays(currentTemp);
    }

    void UpdateDisplays(float temp)
    {
        if (tempText != null)
            tempText.text = temp.ToString("F1");

        float current = refCurrent1 + (temp - refTemp1) * (refCurrent2 - refCurrent1) / (refTemp2 - refTemp1);
        if (currentText != null)
            currentText.text = current.ToString("F2");

        float voltage = refVoltage1 + (temp - refVoltageTemp1) * (refVoltage2 - refVoltage1) / (refVoltageTemp2 - refVoltageTemp1);
        if (voltageText != null)
            voltageText.text = voltage.ToString("F2");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[TriggerEnter] 进入对象名: {other.name}, Tag: {other.tag}");
        if (other.CompareTag(handTag))
        {
            Debug.Log(">> Hand enters");
            heating = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log($"[TriggerExit] 离开对象名: {other.name}, Tag: {other.tag}");
        if (other.CompareTag(handTag))
        {
            Debug.Log(">> Hand exits");
            heating = false;
        }
    }
}

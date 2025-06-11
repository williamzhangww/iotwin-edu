using UnityEngine;
using TMPro;

public class TempHeater : MonoBehaviour
{
    [Header("UI Display")]
    public TextMeshProUGUI tempText;
    public TextMeshProUGUI currentText;

    [Header("Temperature Settings")]
    public float startTemp = 23f;
    public float targetTemp = 32f;
    public float heatDuration = 12f; // second
    public string handTag = "Hand";

    // Current - Temperature: 24.5¡ãC -> 8.40mA£¬33.0¡ãC -> 9.15mA
    private const float refTemp1 = 24.5f;
    private const float refCurrent1 = 8.40f;
    private const float refTemp2 = 33.0f;
    private const float refCurrent2 = 9.15f;

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
        tempText.text = temp.ToString("F1") + " ¡ãC";

        float current = refCurrent1 + (temp - refTemp1) * (refCurrent2 - refCurrent1) / (refTemp2 - refTemp1);
        currentText.text = current.ToString("F2") + " mA";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(handTag))
        {
            Debug.Log(">> Hand enters");
            heating = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(handTag))
        {
            Debug.Log(">> Hand exits");
            heating = false;
        }
    }
}

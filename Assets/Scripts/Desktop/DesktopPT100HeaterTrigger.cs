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
    [Tooltip("热点偏移（x,y）用于调整手型光标与鼠标射线位置的对齐")]
    public Vector2 hotspotOffset = new Vector2(300, 300); // 热点偏移

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

        // 鼠标射线检测是否指向 PT100
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                cursorOverPT100 = true;

                // 按住鼠标左键 -> 启动加热
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

        // 没对准 PT100 时，自动关闭加热
        if (!cursorOverPT100)
            heating = false;

        // 更新鼠标图标
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

        // 平滑升温或降温
        float target = heating ? targetTemp : startTemp;
        float rate = Mathf.Abs(targetTemp - startTemp) / heatDuration;
        currentTemp = Mathf.MoveTowards(currentTemp, target, rate * Time.deltaTime);

        UpdateDisplays(currentTemp);
    }

    void UpdateDisplays(float temp)
    {
        if (tempText != null)
            tempText.text = temp.ToString("F1");

        // 电流 - 温度映射
        float current = 8.40f + (temp - 24.5f) * (9.15f - 8.40f) / (33.0f - 24.5f);
        if (currentText != null)
            currentText.text = current.ToString("F2");

        // 电压 - 温度映射
        float voltage = 2.50f + (temp - 25.0f) * (2.84f - 2.50f) / (28.4f - 25.0f);
        if (voltageText != null)
            voltageText.text = voltage.ToString("F2");
    }
}

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
    [Tooltip("�ȵ�ƫ�ƣ�x,y�����ڵ������͹�����������λ�õĶ���")]
    public Vector2 hotspotOffset = new Vector2(300, 300); // �ȵ�ƫ��

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

        // ������߼���Ƿ�ָ�� PT100
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                cursorOverPT100 = true;

                // ��ס������ -> ��������
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

        // û��׼ PT100 ʱ���Զ��رռ���
        if (!cursorOverPT100)
            heating = false;

        // �������ͼ��
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

        // ƽ�����»���
        float target = heating ? targetTemp : startTemp;
        float rate = Mathf.Abs(targetTemp - startTemp) / heatDuration;
        currentTemp = Mathf.MoveTowards(currentTemp, target, rate * Time.deltaTime);

        UpdateDisplays(currentTemp);
    }

    void UpdateDisplays(float temp)
    {
        if (tempText != null)
            tempText.text = temp.ToString("F1");

        // ���� - �¶�ӳ��
        float current = 8.40f + (temp - 24.5f) * (9.15f - 8.40f) / (33.0f - 24.5f);
        if (currentText != null)
            currentText.text = current.ToString("F2");

        // ��ѹ - �¶�ӳ��
        float voltage = 2.50f + (temp - 25.0f) * (2.84f - 2.50f) / (28.4f - 25.0f);
        if (voltageText != null)
            voltageText.text = voltage.ToString("F2");
    }
}

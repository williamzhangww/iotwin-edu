using UnityEngine;

public class DesktopPowerSwitch : MonoBehaviour
{
    [Header("按钮设置")]
    public Renderer buttonRenderer;
    public Color offColor = Color.red;
    public Color onColor = Color.green;
    public Transform buttonTransform;
    public Vector3 pressOffset = new Vector3(0, -0.005f, 0);

    [Header("显示对象")]
    public LineRenderer laserLine;      // 只控制激光
    public GameObject canvasLCD;
    public GameObject distanceValueObj;

    [Header("音效")]
    public AudioSource audioSource;

    [Header("电源状态")]
    public bool powerOn = false;

    private Vector3 initialPos;

    void Start()
    {
        if (buttonTransform != null)
            initialPos = buttonTransform.localPosition;

        UpdateUI();
    }

    void Update()
    {
        // 鼠标点击检测
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    TogglePower();
                }
            }
        }
    }

    void TogglePower()
    {
        powerOn = !powerOn;

        // 按钮动画
        if (buttonTransform != null)
            buttonTransform.localPosition = initialPos + (powerOn ? pressOffset : Vector3.zero);

        // 改变按钮颜色
        if (buttonRenderer != null)
            buttonRenderer.material.color = powerOn ? onColor : offColor;

        // 激活/隐藏 UI：只隐藏激光，不隐藏整个DT50
        if (laserLine != null) laserLine.enabled = powerOn;   // 控制激光开关
        if (canvasLCD != null) canvasLCD.SetActive(powerOn);
        if (distanceValueObj != null) distanceValueObj.SetActive(powerOn);

        // 播放音效
        if (audioSource != null)
        {
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
}

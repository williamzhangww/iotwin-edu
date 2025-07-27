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

    [Header("鼠标手型设置")]
    public Camera mainCamera;
    public Texture2D handCursor;               // 手型图标
    public Vector2 hotspotOffset = new Vector2(300, 300); // 热点偏移

    [Header("电源状态")]
    public bool powerOn = false;

    private Vector3 initialPos;

    void Start()
    {
        if (buttonTransform != null)
            initialPos = buttonTransform.localPosition;

        if (mainCamera == null)
            mainCamera = Camera.main;

        UpdateUI();
    }

    void Update()
    {
        // 射线检测
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                MouseCursorManager.SetHandCursor();

                if (Input.GetMouseButtonDown(0))
                {
                    TogglePower();
                }
                return;
            }
        }

        // 离开按钮
        MouseCursorManager.ResetCursor();
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
}

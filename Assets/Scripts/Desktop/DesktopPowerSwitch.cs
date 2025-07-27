using UnityEngine;

public class DesktopPowerSwitch : MonoBehaviour
{
    [Header("��ť����")]
    public Renderer buttonRenderer;
    public Color offColor = Color.red;
    public Color onColor = Color.green;
    public Transform buttonTransform;
    public Vector3 pressOffset = new Vector3(0, -0.005f, 0);

    [Header("��ʾ����")]
    public LineRenderer laserLine;      // ֻ���Ƽ���
    public GameObject canvasLCD;
    public GameObject distanceValueObj;

    [Header("��Ч")]
    public AudioSource audioSource;

    [Header("�����������")]
    public Camera mainCamera;
    public Texture2D handCursor;               // ����ͼ��
    public Vector2 hotspotOffset = new Vector2(300, 300); // �ȵ�ƫ��

    [Header("��Դ״̬")]
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
        // ���߼��
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

        // �뿪��ť
        MouseCursorManager.ResetCursor();
    }

    void TogglePower()
    {
        powerOn = !powerOn;

        // ��ť����
        if (buttonTransform != null)
            buttonTransform.localPosition = initialPos + (powerOn ? pressOffset : Vector3.zero);

        // �ı䰴ť��ɫ
        if (buttonRenderer != null)
            buttonRenderer.material.color = powerOn ? onColor : offColor;

        // ����/���� UI��ֻ���ؼ��⣬����������DT50
        if (laserLine != null) laserLine.enabled = powerOn;   // ���Ƽ��⿪��
        if (canvasLCD != null) canvasLCD.SetActive(powerOn);
        if (distanceValueObj != null) distanceValueObj.SetActive(powerOn);

        // ������Ч
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

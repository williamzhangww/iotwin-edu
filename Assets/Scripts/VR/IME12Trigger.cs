using UnityEngine;

public class IME12Trigger : MonoBehaviour
{
    [Header("LED ����")]
    public MeshRenderer ledRenderer;   // �� IME12_LED �� MeshRenderer
    public Material offMaterial;       // �������
    public Material onMaterial;        // ��������

    [Header("Power Switch ����")]
    public PowerSwitchSmooth powerSwitch;   // �� PowerSwitchSmooth �ű��Ķ������

    private bool isInside = false;

    private void OnTriggerEnter(Collider other)
    {
        // �����Դ�ǹر�״̬������Ӧ
        if (powerSwitch != null && !powerSwitch.IsPowerOn())
        {
            SetLedOff();
            return;
        }

        if (other.CompareTag("Metal"))
        {
            isInside = true;
            SetLedOn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Metal"))
        {
            isInside = false;
            SetLedOff();
        }
    }

    private void Update()
    {
        // �����Դ���ˣ�ֱ�ӹص�
        if (powerSwitch != null && !powerSwitch.IsPowerOn())
        {
            SetLedOff();
            return;
        }

        // �����Դ�����Ҳ��ڴ������ڣ���֤�������
        if (!isInside)
        {
            SetLedOff();
        }
    }

    private void SetLedOn()
    {
        if (ledRenderer != null && onMaterial != null)
            ledRenderer.material = onMaterial;
    }

    private void SetLedOff()
    {
        if (ledRenderer != null && offMaterial != null)
            ledRenderer.material = offMaterial;
    }
}

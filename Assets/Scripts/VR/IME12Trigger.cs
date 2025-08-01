using UnityEngine;

public class IME12Trigger : MonoBehaviour
{
    [Header("LED 控制")]
    public MeshRenderer ledRenderer;   // 拖 IME12_LED 的 MeshRenderer
    public Material offMaterial;       // 灯灭材质
    public Material onMaterial;        // 灯亮材质

    [Header("Power Switch 关联")]
    public PowerSwitchSmooth powerSwitch;   // 拖 PowerSwitchSmooth 脚本的对象进来

    private bool isInside = false;

    private void OnTriggerEnter(Collider other)
    {
        // 如果电源是关闭状态，不响应
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
        // 如果电源关了，直接关灯
        if (powerSwitch != null && !powerSwitch.IsPowerOn())
        {
            SetLedOff();
            return;
        }

        // 如果电源开着且不在触发器内，保证灯是灭的
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

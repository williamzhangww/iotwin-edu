using UnityEngine;
using TMPro;

public class DesktopModeUI : MonoBehaviour
{
    public DesktopCubeSelectMove cubeScript;  // Cube 脚本引用
    public TextMeshProUGUI modeText;          // UI 文本引用

    void Update()
    {
        if (cubeScript != null && modeText != null)
        {
            if (cubeScript.IsSelected)
            {
                modeText.text = "[Cube Mode] WASD + Q/E\n用来移动 Cube";
            }
            else
            {
                modeText.text = "[Camera Mode] WASD + Q/E\n用来移动 Camera";
            }
        }
    }
}

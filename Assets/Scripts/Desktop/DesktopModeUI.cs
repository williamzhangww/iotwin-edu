using UnityEngine;
using TMPro;

public class DesktopModeUI : MonoBehaviour
{
    public DesktopCubeSelectMove cubeScript;  // Cube �ű�����
    public TextMeshProUGUI modeText;          // UI �ı�����

    void Update()
    {
        if (cubeScript != null && modeText != null)
        {
            if (cubeScript.IsSelected)
            {
                modeText.text = "[Cube Mode] WASD + Q/E\n�����ƶ� Cube";
            }
            else
            {
                modeText.text = "[Camera Mode] WASD + Q/E\n�����ƶ� Camera";
            }
        }
    }
}

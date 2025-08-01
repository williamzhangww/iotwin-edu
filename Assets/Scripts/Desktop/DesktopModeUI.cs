using UnityEngine;
using TMPro;

public class DesktopModeUI : MonoBehaviour
{
    public DesktopCubeSelectMove cubeScript;  // Reference to the Cube script
    public TextMeshProUGUI modeText;          // Reference to the UI text

    void Update()
    {
        if (cubeScript != null && modeText != null)
        {
            if (cubeScript.IsSelected)
            {
                modeText.text = "[Cube Mode] WASD + Q/E\nUsed to move the Cube";
            }
            else
            {
                modeText.text = "[Camera Mode] WASD + Q/E\nUsed to move the Camera";
            }
        }
    }
}

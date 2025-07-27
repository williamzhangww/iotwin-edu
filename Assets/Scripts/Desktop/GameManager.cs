using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("鼠标手型光标")]
    public Texture2D handCursor;
    public Vector2 hotspotOffset = new Vector2(300, 300);

    void Awake()
    {
        // 初始化全局光标资源
        MouseCursorManager.handCursor = handCursor;
        MouseCursorManager.hotspotOffset = hotspotOffset;
    }
}

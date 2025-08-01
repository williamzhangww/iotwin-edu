using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Mouse Hand Cursor")]
    public Texture2D handCursor;
    public Vector2 hotspotOffset = new Vector2(300, 300);

    void Awake()
    {
        // Initialize global cursor resources
        MouseCursorManager.handCursor = handCursor;
        MouseCursorManager.hotspotOffset = hotspotOffset;
    }
}

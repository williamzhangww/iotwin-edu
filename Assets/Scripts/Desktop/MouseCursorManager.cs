using UnityEngine;

public static class MouseCursorManager
{
    // Hand cursor resource (set during initialization)
    public static Texture2D handCursor;
    public static Vector2 hotspotOffset = new Vector2(300, 300);

    /// <summary>
    /// Set the cursor to the hand icon
    /// </summary>
    public static void SetHandCursor()
    {
        if (handCursor != null)
        {
            Cursor.SetCursor(handCursor, hotspotOffset, CursorMode.Auto);
        }
    }

    /// <summary>
    /// Reset the cursor to the default
    /// </summary>
    public static void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}

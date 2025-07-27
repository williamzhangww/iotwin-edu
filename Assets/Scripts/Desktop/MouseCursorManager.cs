using UnityEngine;

public static class MouseCursorManager
{
    // 手型光标资源（初始化时设置）
    public static Texture2D handCursor;
    public static Vector2 hotspotOffset = new Vector2(300, 300);

    /// <summary>
    /// 设置为手型光标
    /// </summary>
    public static void SetHandCursor()
    {
        if (handCursor != null)
        {
            Cursor.SetCursor(handCursor, hotspotOffset, CursorMode.Auto);
        }
    }

    /// <summary>
    /// 恢复默认光标
    /// </summary>
    public static void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}

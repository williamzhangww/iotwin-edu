using UnityEngine;

public static class MouseCursorManager
{
    // ���͹����Դ����ʼ��ʱ���ã�
    public static Texture2D handCursor;
    public static Vector2 hotspotOffset = new Vector2(300, 300);

    /// <summary>
    /// ����Ϊ���͹��
    /// </summary>
    public static void SetHandCursor()
    {
        if (handCursor != null)
        {
            Cursor.SetCursor(handCursor, hotspotOffset, CursorMode.Auto);
        }
    }

    /// <summary>
    /// �ָ�Ĭ�Ϲ��
    /// </summary>
    public static void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}

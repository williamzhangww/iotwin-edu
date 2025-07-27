using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("������͹��")]
    public Texture2D handCursor;
    public Vector2 hotspotOffset = new Vector2(300, 300);

    void Awake()
    {
        // ��ʼ��ȫ�ֹ����Դ
        MouseCursorManager.handCursor = handCursor;
        MouseCursorManager.hotspotOffset = hotspotOffset;
    }
}

using UnityEngine;

public class CubeStickyToWall : MonoBehaviour
{
    private bool isOnWall = false;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Board")
        {
            transform.SetParent(collision.transform.root); // ����Ϊ Wall v1 ��������
            isOnWall = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Board" && isOnWall)
        {
            transform.SetParent(null); // ����
            isOnWall = false;
        }
    }
}

using UnityEngine;

public class CubeStickyToWall : MonoBehaviour
{
    private bool isOnWall = false;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Body4")
        {
            transform.SetParent(collision.transform.root); // 设置为 Wall v1 的子物体
            isOnWall = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Body4" && isOnWall)
        {
            transform.SetParent(null); // 脱离
            isOnWall = false;
        }
    }
}

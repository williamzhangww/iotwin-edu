using UnityEngine;

public class CubeStickyToWall : MonoBehaviour
{
    private bool isOnWall = false;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Board")
        {
            // Set as a child of Wall
            transform.SetParent(collision.transform.root);
            isOnWall = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Board" && isOnWall)
        {
            // Detach from the wall
            transform.SetParent(null);
            isOnWall = false;
        }
    }
}

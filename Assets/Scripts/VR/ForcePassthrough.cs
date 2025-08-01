using UnityEngine;

public class ForcePassthrough : MonoBehaviour
{
    void Start()
    {
        if (OVRManager.instance != null)
            OVRManager.instance.isInsightPassthroughEnabled = true;
    }
}

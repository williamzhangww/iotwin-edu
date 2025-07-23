using UnityEngine;
using Oculus.Interaction;

public class PlaneGrabTransformer : MonoBehaviour, ITransformer
{
    private IGrabbable _grabbable;
    private Pose _initialGrabPose;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;

    public void Initialize(IGrabbable grabbable)
    {
        _grabbable = grabbable;
    }

    public void BeginTransform()
    {
        _initialGrabPose = _grabbable.GrabPoints[0];
        _initialPosition = _grabbable.Transform.position;
        _initialRotation = _grabbable.Transform.rotation;
    }

    public void UpdateTransform()
    {
        Pose currentPose = _grabbable.GrabPoints[0];

        // 获取旋转变化
        Quaternion deltaRotation = currentPose.rotation * Quaternion.Inverse(_initialGrabPose.rotation);
        float yAngle = deltaRotation.eulerAngles.y;

        // 构造新的旋转：只保留 Y 轴变化
        Quaternion rotation = Quaternion.Euler(0f, yAngle, 0f);

        // 用旋转作用在初始相对位移上
        Vector3 initialOffset = _initialPosition - _initialGrabPose.position;
        Vector3 rotatedOffset = rotation * initialOffset;

        // 设置新位置（抓取点不动，物体绕它转）
        Vector3 newPosition = currentPose.position + rotatedOffset;
        newPosition.y = _initialPosition.y; // 锁定 Y 坐标

        _grabbable.Transform.SetPositionAndRotation(newPosition, rotation * _initialRotation);
    }


    public void EndTransform()
    {
        // Nothing needed here
    }
}

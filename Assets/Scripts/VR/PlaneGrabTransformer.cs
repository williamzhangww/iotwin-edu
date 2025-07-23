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

        // ��ȡ��ת�仯
        Quaternion deltaRotation = currentPose.rotation * Quaternion.Inverse(_initialGrabPose.rotation);
        float yAngle = deltaRotation.eulerAngles.y;

        // �����µ���ת��ֻ���� Y ��仯
        Quaternion rotation = Quaternion.Euler(0f, yAngle, 0f);

        // ����ת�����ڳ�ʼ���λ����
        Vector3 initialOffset = _initialPosition - _initialGrabPose.position;
        Vector3 rotatedOffset = rotation * initialOffset;

        // ������λ�ã�ץȡ�㲻������������ת��
        Vector3 newPosition = currentPose.position + rotatedOffset;
        newPosition.y = _initialPosition.y; // ���� Y ����

        _grabbable.Transform.SetPositionAndRotation(newPosition, rotation * _initialRotation);
    }


    public void EndTransform()
    {
        // Nothing needed here
    }
}

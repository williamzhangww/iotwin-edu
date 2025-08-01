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

        // Get the rotation delta
        Quaternion deltaRotation = currentPose.rotation * Quaternion.Inverse(_initialGrabPose.rotation);
        float yAngle = deltaRotation.eulerAngles.y;

        // Construct a new rotation: only keep Y-axis change
        Quaternion rotation = Quaternion.Euler(0f, yAngle, 0f);

        // Apply the rotation to the initial relative offset
        Vector3 initialOffset = _initialPosition - _initialGrabPose.position;
        Vector3 rotatedOffset = rotation * initialOffset;

        // Set the new position (grab point stays fixed, object rotates around it)
        Vector3 newPosition = currentPose.position + rotatedOffset;
        newPosition.y = _initialPosition.y; // Lock Y coordinate

        _grabbable.Transform.SetPositionAndRotation(newPosition, rotation * _initialRotation);
    }

    public void EndTransform()
    {
        // Nothing needed here
    }
}

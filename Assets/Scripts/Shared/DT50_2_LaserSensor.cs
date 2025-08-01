using UnityEngine;
using TMPro;

[RequireComponent(typeof(LineRenderer))]
public class DT50_2_LaserSensor : MonoBehaviour
{
    [Header("References")]
    public Transform laserOrigin;         // Laser emission point (empty GameObject)
    public Transform laserSpot;           // Hit spot (Quad or empty GameObject)
    public TextMeshPro distanceText;      // Floating text (your original DistanceValue)

    [Header("Settings")]
    public float minDistance = 0.2f;      // Minimum distance (meters)
    public float maxDistance = 30f;       // Maximum distance (meters)
    public LayerMask raycastMask;         // Target layers to detect (exclude hand/body)

    private LineRenderer laserLine;

    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        laserLine.positionCount = 2;
        laserLine.startWidth = 0.001f;
        laserLine.endWidth = 0.001f;

        if (distanceText != null)
            distanceText.gameObject.SetActive(false); // Initially hidden
    }

    void Update()
    {
        Vector3 origin = laserOrigin ? laserOrigin.position : transform.position;
        Vector3 direction = laserOrigin ? laserOrigin.right : transform.right;

        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        laserLine.SetPosition(0, origin); // Laser start point

        if (Physics.Raycast(ray, out hit, maxDistance, raycastMask))
        {
            float distance = hit.distance;
            Vector3 hitPoint = hit.point;

            // Set laser end point to the hit point
            laserLine.SetPosition(1, hitPoint);

            // Show laser spot
            if (laserSpot != null)
            {
                laserSpot.position = hitPoint + hit.normal * 0.001f;
                laserSpot.rotation = Quaternion.LookRotation(-hit.normal);
                laserSpot.gameObject.SetActive(true);
            }

            // Show distance text if within valid range
            if (distance >= minDistance && distance <= maxDistance && distanceText != null)
            {
                distanceText.text = $"{(distance * 1000f):F0} mm"; // Millimeter integer
                Vector3 camRight = Camera.main.transform.right;
                distanceText.transform.position = hitPoint + hit.normal * 0.11f - camRight * 0.03f;
                distanceText.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward); // Face the camera
                distanceText.gameObject.SetActive(true);
            }
            else if (distanceText != null)
            {
                distanceText.gameObject.SetActive(false);
            }
        }
        else
        {
            // No hit ¡ú laser at full length, hide spot and label
            laserLine.SetPosition(1, origin + direction * maxDistance);

            if (laserSpot != null)
                laserSpot.gameObject.SetActive(false);

            if (distanceText != null)
                distanceText.gameObject.SetActive(false);
        }
    }
}

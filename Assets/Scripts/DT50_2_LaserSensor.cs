using UnityEngine;
using TMPro;

[RequireComponent(typeof(LineRenderer))]
public class DT50_2_LaserSensor : MonoBehaviour
{
    [Header("References")]
    public Transform laserOrigin;         // ���ⷢ��㣨�����壩
    public Transform laserSpot;           // ���й�ߣ�Quad ������壩
    public TextMeshPro distanceText;      // �������֣���ԭ���� DistanceValue��

    [Header("Settings")]
    public float minDistance = 0.2f;      // ��С��ࣨ�ף�
    public float maxDistance = 30f;       // ����ࣨ�ף�
    public LayerMask raycastMask;         // �ɼ���Ŀ��㣨�ų���/���壩

    private LineRenderer laserLine;

    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        laserLine.positionCount = 2;
        laserLine.startWidth = 0.001f;
        laserLine.endWidth = 0.001f;

        if (distanceText != null)
            distanceText.gameObject.SetActive(false); // ��ʼ����
    }

    void Update()
    {
        Vector3 origin = laserOrigin ? laserOrigin.position : transform.position;
        Vector3 direction = laserOrigin ? laserOrigin.right : transform.right;

        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        laserLine.SetPosition(0, origin); // �������

        if (Physics.Raycast(ray, out hit, maxDistance, raycastMask))
        {
            float distance = hit.distance;
            Vector3 hitPoint = hit.point;

            // ���ü����յ�Ϊ���е�
            laserLine.SetPosition(1, hitPoint);

            // ��ʾ������
            if (laserSpot != null)
            {
                laserSpot.position = hitPoint + hit.normal * 0.001f;
                laserSpot.rotation = Quaternion.LookRotation(-hit.normal);
                laserSpot.gameObject.SetActive(true);
            }

            // ��Ч��������ʾ��������
            if (distance >= minDistance && distance <= maxDistance && distanceText != null)
            {
                distanceText.text = $"{(distance * 1000f):F0} mm"; // ��������
                Vector3 camRight = Camera.main.transform.right;
                distanceText.transform.position = hitPoint + hit.normal * 0.11f - camRight * 0.03f;
                distanceText.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward); // ��������ͷ
                distanceText.gameObject.SetActive(true);
            }
            else if (distanceText != null)
            {
                distanceText.gameObject.SetActive(false);
            }
        }
        else
        {
            // û���������� �� ����������������أ���ǩ����
            laserLine.SetPosition(1, origin + direction * maxDistance);

            if (laserSpot != null)
                laserSpot.gameObject.SetActive(false);

            if (distanceText != null)
                distanceText.gameObject.SetActive(false);
        }
    }
}

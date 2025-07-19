using UnityEngine;
using TMPro;

[RequireComponent(typeof(LineRenderer))]
public class DT50_2_LaserSensor : MonoBehaviour
{
    [Header("References")]
    public Transform laserOrigin;         // 激光发射点（空物体）
    public Transform laserSpot;           // 命中光斑（Quad 或空物体）
    public TextMeshPro distanceText;      // 浮动文字（你原来的 DistanceValue）

    [Header("Settings")]
    public float minDistance = 0.2f;      // 最小测距（米）
    public float maxDistance = 30f;       // 最大测距（米）
    public LayerMask raycastMask;         // 可检测的目标层（排除手/身体）

    private LineRenderer laserLine;

    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        laserLine.positionCount = 2;
        laserLine.startWidth = 0.001f;
        laserLine.endWidth = 0.001f;

        if (distanceText != null)
            distanceText.gameObject.SetActive(false); // 初始隐藏
    }

    void Update()
    {
        Vector3 origin = laserOrigin ? laserOrigin.position : transform.position;
        Vector3 direction = laserOrigin ? laserOrigin.right : transform.right;

        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        laserLine.SetPosition(0, origin); // 激光起点

        if (Physics.Raycast(ray, out hit, maxDistance, raycastMask))
        {
            float distance = hit.distance;
            Vector3 hitPoint = hit.point;

            // 设置激光终点为命中点
            laserLine.SetPosition(1, hitPoint);

            // 显示激光光斑
            if (laserSpot != null)
            {
                laserSpot.position = hitPoint + hit.normal * 0.001f;
                laserSpot.rotation = Quaternion.LookRotation(-hit.normal);
                laserSpot.gameObject.SetActive(true);
            }

            // 有效距离内显示距离文字
            if (distance >= minDistance && distance <= maxDistance && distanceText != null)
            {
                distanceText.text = $"{(distance * 1000f):F0} mm"; // 毫米整数
                Vector3 camRight = Camera.main.transform.right;
                distanceText.transform.position = hitPoint + hit.normal * 0.11f - camRight * 0.03f;
                distanceText.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward); // 面向摄像头
                distanceText.gameObject.SetActive(true);
            }
            else if (distanceText != null)
            {
                distanceText.gameObject.SetActive(false);
            }
        }
        else
        {
            // 没有命中物体 → 激光拉满，光斑隐藏，标签隐藏
            laserLine.SetPosition(1, origin + direction * maxDistance);

            if (laserSpot != null)
                laserSpot.gameObject.SetActive(false);

            if (distanceText != null)
                distanceText.gameObject.SetActive(false);
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class DesktopPlayerController : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    public float lookSpeed = 2.0f;
    public Transform cameraTransform;

    private CharacterController controller;
    private float rotationX = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        LockCursor(); // ��ʼ�����ڴ�����
        Cursor.visible = true; // ʼ����ʾ���
    }

    void Update()
    {
        HandleMouseLock();

        // ������걻����ʱ��������ת
        if (Cursor.lockState == CursorLockMode.Confined && Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

            transform.Rotate(Vector3.up * mouseX);
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f);

            if (cameraTransform != null)
                cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        }

        // �ƶ����ƣ�WASD��
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = transform.right * h + transform.forward * v;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void HandleMouseLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor(); // ��������Ƴ�����
        }
        else if (Input.GetMouseButtonDown(1))
        {
            LockCursor(); // �Ҽ��ָ�����
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined; // ��������ڴ�����
        Cursor.visible = true;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None; // �����Ƴ�����
        Cursor.visible = true;
    }
}

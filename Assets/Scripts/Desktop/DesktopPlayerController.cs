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
        LockCursor(); // 初始锁定在窗口内
        Cursor.visible = true; // 始终显示鼠标
    }

    void Update()
    {
        HandleMouseLock();

        // 仅当鼠标被锁定时才允许旋转
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

        // 移动控制（WASD）
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = transform.right * h + transform.forward * v;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void HandleMouseLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor(); // 允许鼠标移出窗口
        }
        else if (Input.GetMouseButtonDown(1))
        {
            LockCursor(); // 右键恢复锁定
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined; // 限制鼠标在窗口内
        Cursor.visible = true;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None; // 鼠标可移出窗口
        Cursor.visible = true;
    }
}

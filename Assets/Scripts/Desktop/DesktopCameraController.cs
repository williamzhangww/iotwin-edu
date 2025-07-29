using UnityEngine;
using TMPro;

public class DesktopCameraController : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    public float lookSpeed = 2.0f;
    public Transform cameraTransform;

    [Header("UI 设置")]
    public TextMeshProUGUI modeText;  // 左下角 UI 文本

    private CharacterController controller;
    private float rotationX = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        LockCursor();
        Cursor.visible = true;
        UpdateModeText(false); // 默认 Camera Mode
    }

    void Update()
    {
        HandleMouseLock();

        // 鼠标右键旋转 (两种模式都允许)
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

        // 检查是否有 Cube 被选中
        bool anyCubeSelected = IsAnyCubeSelected();

        // 更新左下角 UI 提示
        UpdateModeText(anyCubeSelected);

        //  只有没有 Cube 被选中时，Camera 才能移动
        if (!anyCubeSelected)
        {
            HandleCameraMove();
        }
    }

    bool IsAnyCubeSelected()
    {
#if UNITY_2023_1_OR_NEWER
        DesktopCubeSelectMove[] cubes = Object.FindObjectsByType<DesktopCubeSelectMove>(FindObjectsSortMode.None);
#else
        DesktopCubeSelectMove[] cubes = Object.FindObjectsOfType<DesktopCubeSelectMove>();
#endif
        foreach (var cube in cubes)
        {
            if (cube.IsSelected) return true;
        }
        return false;
    }

    void HandleCameraMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Q / E 控制上下移动
        float y = 0f;
        if (Input.GetKey(KeyCode.Q)) y = 1f;
        if (Input.GetKey(KeyCode.E)) y = -1f;

        Vector3 move = transform.right * h + transform.forward * v + transform.up * y;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void HandleMouseLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            LockCursor();
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// 更新左下角 UI 提示
    /// </summary>
    void UpdateModeText(bool isCubeMode)
    {
        if (modeText == null) return;

        if (isCubeMode)
            modeText.text = "[Cube Mode] WASD + Q/E";
        else
            modeText.text = "[Camera Mode] WASD + Q/E";
    }
}

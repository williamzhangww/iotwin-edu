using UnityEngine;
using TMPro;

public class DesktopCameraController : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    public float lookSpeed = 2.0f;
    public Transform cameraTransform;

    [Header("UI Settings")]
    public TextMeshProUGUI modeText;  // Bottom-left UI text

    private CharacterController controller;
    private float rotationX = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        UpdateModeText(false); // Default to Camera Mode
    }

    void Update()
    {
        HandleMouseLock();

        // Right mouse button: rotate view
        if (Cursor.lockState == CursorLockMode.Locked && Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

            transform.Rotate(Vector3.up * mouseX);
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f);

            if (cameraTransform != null)
                cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        }

        // Check if any Cube is selected
        bool anyCubeSelected = IsAnyCubeSelected();

        // Update mode text on UI
        UpdateModeText(anyCubeSelected);

        // Allow camera movement only when no Cube is selected
        if (!anyCubeSelected)
        {
            HandleCameraMove();
        }
    }

    /// <summary>
    /// Check if any Cube is currently selected
    /// </summary>
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

    /// <summary>
    /// Camera movement (only when no Cube is selected)
    /// </summary>
    void HandleCameraMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float y = 0f;
        if (Input.GetKey(KeyCode.Q)) y = 1f;
        if (Input.GetKey(KeyCode.E)) y = -1f;

        Vector3 move = transform.right * h + transform.forward * v + transform.up * y;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Handle mouse lock logic
    /// </summary>
    void HandleMouseLock()
    {
        if (Input.GetMouseButton(1))
        {
            LockCursor();
        }
        else
        {
            UnlockCursor();
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Update the bottom-left mode text
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

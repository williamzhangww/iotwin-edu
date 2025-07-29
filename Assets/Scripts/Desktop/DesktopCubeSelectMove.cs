using UnityEngine;

public class DesktopCubeSelectMove : MonoBehaviour
{
    [Header("Cube 移动参数")]
    public float moveSpeed = 0.5f;
    public Color highlightColor = Color.yellow;

    private Color originalColor;
    private Renderer cubeRenderer;
    private bool isSelected = false;

    public bool IsSelected => isSelected;

    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        if (cubeRenderer != null)
            originalColor = cubeRenderer.material.color;
    }

    void Update()
    {
        HandleMouseHoverAndClick();

        //  选中时 WASD+QE 永远有效（不依赖鼠标悬停状态）
        if (isSelected)
        {
            HandleMoveWorld();
        }
    }

    /// <summary>
    /// 检查鼠标悬停和点击
    /// </summary>
    void HandleMouseHoverAndClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                // 鼠标悬停时显示手型
                MouseCursorManager.SetHandCursor();

                // 鼠标左键点击 → 切换选中
                if (Input.GetMouseButtonDown(0))
                {
                    DeselectAllCubesExceptThis();
                    ToggleSelect(true);
                }
                return; // 停止 ResetCursor
            }
        }

        // 鼠标不在 Cube 上 → 恢复默认鼠标
        MouseCursorManager.ResetCursor();

        // 鼠标点击空白处 → 取消选中
        if (Input.GetMouseButtonDown(0))
        {
            ToggleSelect(false);
        }
    }

    void HandleMoveWorld()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) move += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) move += Vector3.back;
        if (Input.GetKey(KeyCode.A)) move += Vector3.left;
        if (Input.GetKey(KeyCode.D)) move += Vector3.right;
        if (Input.GetKey(KeyCode.Q)) move += Vector3.up;
        if (Input.GetKey(KeyCode.E)) move += Vector3.down;

        transform.position += move * moveSpeed * Time.deltaTime;
    }

    void ToggleSelect(bool select)
    {
        isSelected = select;
        if (cubeRenderer != null)
        {
            cubeRenderer.material.color = isSelected ? highlightColor : originalColor;
        }
    }

    void DeselectAllCubesExceptThis()
    {
#if UNITY_2023_1_OR_NEWER
        DesktopCubeSelectMove[] cubes = Object.FindObjectsByType<DesktopCubeSelectMove>(FindObjectsSortMode.None);
#else
        DesktopCubeSelectMove[] cubes = Object.FindObjectsOfType<DesktopCubeSelectMove>();
#endif
        foreach (var cube in cubes)
        {
            if (cube != this)
                cube.ToggleSelect(false);
        }
    }
}

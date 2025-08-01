using UnityEngine;

public class DesktopCubeSelectMove : MonoBehaviour
{
    [Header("Cube Movement Parameters")]
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

        // When selected, WASD + Q/E is always effective (independent of mouse hover state)
        if (isSelected)
        {
            HandleMoveWorld();
        }
    }

    /// <summary>
    /// Check for mouse hover and click
    /// </summary>
    void HandleMouseHoverAndClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                // Show hand cursor when hovering with the mouse
                MouseCursorManager.SetHandCursor();

                // Left mouse click ¡ú toggle selection
                if (Input.GetMouseButtonDown(0))
                {
                    DeselectAllCubesExceptThis();
                    ToggleSelect(true);
                }
                return; // Stop ResetCursor
            }
        }

        // Mouse not over the Cube ¡ú reset to default cursor
        MouseCursorManager.ResetCursor();

        // Mouse click on empty area ¡ú deselect
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

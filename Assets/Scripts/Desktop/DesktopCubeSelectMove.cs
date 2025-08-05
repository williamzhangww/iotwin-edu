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

    [Header("Hint Hiding")]
    public HideHintOnCubeMove hintHider;  // Reference to hint-hiding script on the same object

    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        if (cubeRenderer != null)
            originalColor = cubeRenderer.material.color;
    }

    void Update()
    {
        HandleMouseHoverAndClick();

        // When selected, WASD + Q/E controls movement
        if (isSelected)
        {
            HandleMoveWorld();
        }
    }

    /// <summary>
    /// Handle mouse hover and selection click
    /// </summary>
    void HandleMouseHoverAndClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                MouseCursorManager.SetHandCursor();

                if (Input.GetMouseButtonDown(0))
                {
                    DeselectAllCubesExceptThis();
                    ToggleSelect(true);
                }
                return;
            }
        }

        MouseCursorManager.ResetCursor();

        if (Input.GetMouseButtonDown(0))
        {
            ToggleSelect(false);
        }
    }

    /// <summary>
    /// Move cube in world space using WASD + Q/E
    /// </summary>
    void HandleMoveWorld()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) move += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) move += Vector3.back;
        if (Input.GetKey(KeyCode.A)) move += Vector3.left;
        if (Input.GetKey(KeyCode.D)) move += Vector3.right;
        if (Input.GetKey(KeyCode.Q)) move += Vector3.up;
        if (Input.GetKey(KeyCode.E)) move += Vector3.down;

        if (move != Vector3.zero)
        {
            transform.position += move * moveSpeed * Time.deltaTime;

            // Notify hint hider to start countdown
            if (hintHider != null)
            {
                hintHider.TriggerHideCountdown();
            }
        }
    }

    /// <summary>
    /// Toggle selection state and update color
    /// </summary>
    void ToggleSelect(bool select)
    {
        isSelected = select;
        if (cubeRenderer != null)
        {
            cubeRenderer.material.color = isSelected ? highlightColor : originalColor;
        }
    }

    /// <summary>
    /// Deselect all other cubes except this one
    /// </summary>
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

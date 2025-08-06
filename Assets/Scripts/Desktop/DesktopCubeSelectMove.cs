using UnityEngine;

public class DesktopCubeSelectMove : MonoBehaviour, IMouseHoverInteractable
{
    [Header("Cube Movement Parameters")]
    public float moveSpeed = 0.5f;
    public Color highlightColor = Color.yellow;

    [Header("Hint Hiding")]
    public HideHintOnCubeMove hintHider;  // Hint controller script

    private Color originalColor;
    private Renderer cubeRenderer;
    private bool isSelected = false;
    private bool isHovering = false;

    public bool IsSelected => isSelected;

    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        if (cubeRenderer != null)
            originalColor = cubeRenderer.material.color;
    }

    void Update()
    {
        if (isSelected)
        {
            HandleMoveWorld();
        }

        // Click on empty area to deselect
        if (!isHovering && Input.GetMouseButtonDown(0))
        {
            ToggleSelect(false);
        }
    }

    /// <summary>
    /// Handles cube movement
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

            // Notify the hint script to start hiding countdown
            if (hintHider != null)
                hintHider.TriggerHideCountdown();
        }
    }

    /// <summary>
    /// Toggle selection state
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
    /// Deselect all other cubes
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

    // --- Mouse hover interface (called by MouseHoverDispatcher) ---
    public void OnMouseHoverEnter()
    {
        isHovering = true;
        MouseCursorManager.SetHandCursor();
    }

    public void OnMouseHoverExit()
    {
        isHovering = false;
        MouseCursorManager.ResetCursor();
    }

    public void OnMouseClick()
    {
        DeselectAllCubesExceptThis();
        ToggleSelect(true);
    }
}

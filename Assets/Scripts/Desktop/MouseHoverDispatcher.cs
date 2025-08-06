using UnityEngine;

public class MouseHoverDispatcher : MonoBehaviour
{
    private IMouseHoverInteractable currentHover;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera == null) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var interactable = hit.collider.GetComponent<IMouseHoverInteractable>();
            if (interactable != null)
            {
                if (currentHover != interactable)
                {
                    currentHover?.OnMouseHoverExit();
                    currentHover = interactable;
                    currentHover.OnMouseHoverEnter();
                }

                if (Input.GetMouseButtonDown(0))
                    currentHover.OnMouseClick();

                return;
            }
        }

        // If no interactable was hit or the pointer left the current one
        if (currentHover != null)
        {
            currentHover.OnMouseHoverExit();
            currentHover = null;
        }
    }
}

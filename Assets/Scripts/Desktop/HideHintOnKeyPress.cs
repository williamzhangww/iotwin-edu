using UnityEngine;
using System.Collections;

public class HideHintOnKeyPress : MonoBehaviour
{
    [SerializeField] private GameObject hintTextObject;  // Hint text to show/hide
    [SerializeField] private float hideDelay = 3f;       // Delay before hiding the hint

    private bool hasStartedHideCountdown = false;

    void Start()
    {
        if (hintTextObject != null)
            hintTextObject.SetActive(true);
    }

    void Update()
    {
        if (!hasStartedHideCountdown && AnyMovementKeyPressed() && !IsAnyCubeSelected())
        {
            hasStartedHideCountdown = true;
            StartCoroutine(HideAfterDelay());
        }
    }

    private bool AnyMovementKeyPressed()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
               Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
               Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E);
    }

    private bool IsAnyCubeSelected()
    {
#if UNITY_2023_1_OR_NEWER
        var cubes = Object.FindObjectsByType<DesktopCubeSelectMove>(FindObjectsSortMode.None);
#else
        var cubes = Object.FindObjectsOfType<DesktopCubeSelectMove>();
#endif
        foreach (var cube in cubes)
        {
            if (cube.IsSelected) return true;
        }
        return false;
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);

        if (hintTextObject != null)
            hintTextObject.SetActive(false);
    }
}

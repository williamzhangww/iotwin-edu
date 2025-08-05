using System.Collections;
using UnityEngine;

public class HideHintOnKeyPress : MonoBehaviour
{
    [SerializeField] private GameObject hintTextObject;  // Hint text to display
    [SerializeField] private float hideDelay = 3f;       // Delay before hiding the hint

    private bool hasStartedHideCountdown = false;

    void Start()
    {
        if (hintTextObject != null)
        {
            hintTextObject.SetActive(true); // Show hint at startup
        }
    }

    void Update()
    {
        if (!hasStartedHideCountdown && IsMovementKeyPressed())
        {
            hasStartedHideCountdown = true;
            StartCoroutine(HideAfterDelay());
        }
    }

    /// <summary>
    /// Check if any movement key has been pressed (WASD + Q/E)
    /// </summary>
    bool IsMovementKeyPressed()
    {
        return Input.GetKeyDown(KeyCode.W) ||
               Input.GetKeyDown(KeyCode.A) ||
               Input.GetKeyDown(KeyCode.S) ||
               Input.GetKeyDown(KeyCode.D) ||
               Input.GetKeyDown(KeyCode.Q) ||
               Input.GetKeyDown(KeyCode.E);
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);
        if (hintTextObject != null)
        {
            hintTextObject.SetActive(false);
        }
    }
}

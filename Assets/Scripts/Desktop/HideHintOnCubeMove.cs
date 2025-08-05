using UnityEngine;
using System.Collections;

public class HideHintOnCubeMove : MonoBehaviour
{
    [SerializeField] private GameObject hintTextObject;  // Hint text to show/hide
    [SerializeField] private float hideDelay = 3f;       // Delay before hiding the hint

    private bool hasStartedHideCountdown = false;

    void Start()
    {
        if (hintTextObject != null)
        {
            hintTextObject.SetActive(true); // Show hint at startup
        }
    }

    /// <summary>
    /// Called externally to start the countdown for hiding the hint.
    /// </summary>
    public void TriggerHideCountdown()
    {
        if (!hasStartedHideCountdown)
        {
            hasStartedHideCountdown = true;
            StartCoroutine(HideAfterDelay());
        }
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

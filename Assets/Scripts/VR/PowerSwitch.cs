using UnityEngine;
using System.Collections;

public class PowerSwitchSmooth : MonoBehaviour
{
    [Header("Laser & Sound")]
    public LineRenderer laserLine;
    public AudioSource buttonAudio;

    [Header("Button Movement")]
    public Transform redButtonTransform;
    public float pressedY = 0.006f;
    public float animationDuration = 0.1f;

    [Header("Button Light Effect")]
    public Renderer redButtonRenderer;
    public Color powerOffColor = new Color(200f / 255f, 0f, 0f);  // #C80000
    public Color powerOnColor = new Color(0f, 160f / 255f, 0f);   // #00A000

    [Header("LCD Display Control")]
    public GameObject distanceValue;  // Parent object for Distance under DT50
    public GameObject canvasLCD;      // Entire LCD panel

    [Header("Hint Text")]
    public GameObject hintTextObject;  // Hint text object
    public float hintHideDelay = 3f;   // Delay time to hide the hint after pressing

    // Default is Power Off state
    private bool isPowerOn = false;
    private bool isAnimating = false;
    private Vector3 originalPosition;
    private Vector3 pressedPosition;
    private bool hasStartedHide = false;

    private void Start()
    {
        if (redButtonTransform != null)
        {
            originalPosition = redButtonTransform.localPosition;
            pressedPosition = new Vector3(originalPosition.x, pressedY, originalPosition.z);
        }

        // Initialize to Power Off state
        UpdateButtonColor();
        UpdateDisplayStatus();

        if (laserLine != null)
            laserLine.enabled = isPowerOn;

        // Initially display the hint
        if (hintTextObject != null)
        {
            hintTextObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAnimating) return;

        if (other.CompareTag("Hand") || other.name.Contains("Hand") || other.name.Contains("Interactor"))
        {
            TogglePower();
            StartCoroutine(AnimatePressAndRelease());

            // Start countdown to hide the hint on the first press
            if (!hasStartedHide)
            {
                hasStartedHide = true;
                StartCoroutine(HideHintAfterDelay());
            }
        }
    }

    private void TogglePower()
    {
        isPowerOn = !isPowerOn;

        if (laserLine != null)
            laserLine.enabled = isPowerOn;

        if (buttonAudio != null)
        {
            buttonAudio.Stop();
            buttonAudio.Play();
        }

        UpdateButtonColor();
        UpdateDisplayStatus();
    }

    private void UpdateButtonColor()
    {
        if (redButtonRenderer != null && redButtonRenderer.material != null)
        {
            redButtonRenderer.material.color = isPowerOn ? powerOnColor : powerOffColor;
        }
    }

    private void UpdateDisplayStatus()
    {
        if (distanceValue != null)
            distanceValue.SetActive(isPowerOn);

        if (canvasLCD != null)
            canvasLCD.SetActive(isPowerOn);
    }

    private IEnumerator AnimatePressAndRelease()
    {
        isAnimating = true;

        yield return MoveButton(redButtonTransform.localPosition, pressedPosition);
        yield return new WaitForSeconds(0.05f);
        yield return MoveButton(pressedPosition, originalPosition);

        isAnimating = false;
    }

    private IEnumerator MoveButton(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            float t = elapsed / animationDuration;
            redButtonTransform.localPosition = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        redButtonTransform.localPosition = to;
    }

    // Coroutine to hide the hint text
    private IEnumerator HideHintAfterDelay()
    {
        yield return new WaitForSeconds(hintHideDelay);

        if (hintTextObject != null)
        {
            hintTextObject.SetActive(false);
        }
    }

    // Public method to expose power state
    public bool IsPowerOn()
    {
        return isPowerOn;
    }
}

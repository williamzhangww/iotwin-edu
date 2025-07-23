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
    public GameObject distanceValue;  // DT50下方Distance父物体
    public GameObject canvasLCD;      // LCD整块画面

    // 默认是Power Off状态
    private bool isLaserOn = false;
    private bool isAnimating = false;
    private Vector3 originalPosition;
    private Vector3 pressedPosition;

    private void Start()
    {
        if (redButtonTransform != null)
        {
            originalPosition = redButtonTransform.localPosition;
            pressedPosition = new Vector3(originalPosition.x, pressedY, originalPosition.z);
        }

        // 初始化状态为Power Off
        UpdateButtonColor();
        UpdateDisplayStatus();

        if (laserLine != null)
            laserLine.enabled = isLaserOn;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAnimating) return;

        if (other.CompareTag("Hand") || other.name.Contains("Hand") || other.name.Contains("Interactor"))
        {
            ToggleLaser();
            StartCoroutine(AnimatePressAndRelease());
        }
    }

    private void ToggleLaser()
    {
        isLaserOn = !isLaserOn;

        if (laserLine != null)
            laserLine.enabled = isLaserOn;

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
            redButtonRenderer.material.color = isLaserOn ? powerOnColor : powerOffColor;
        }
    }

    private void UpdateDisplayStatus()
    {
        if (distanceValue != null)
            distanceValue.SetActive(isLaserOn);

        if (canvasLCD != null)
            canvasLCD.SetActive(isLaserOn);
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
}

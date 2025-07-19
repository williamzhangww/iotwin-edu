using System.Collections;
using UnityEngine;
using Oculus.Interaction;

public class HideHintOnGrab : MonoBehaviour
{
    [SerializeField] private GameObject hintTextObject; // 提示文字对象
    [SerializeField] private float hideDelay = 5f;      // 抓取后延迟隐藏时间

    private Grabbable grabbable;
    private bool hasStartedHideCountdown = false;

    private void Awake()
    {
        grabbable = GetComponent<Grabbable>();
    }

    private void Start()
    {
        if (hintTextObject != null)
        {
            hintTextObject.SetActive(true); // 初始显示
        }
    }

    private void OnEnable()
    {
        if (grabbable != null)
        {
            grabbable.WhenPointerEventRaised += OnPointerEvent;
        }
    }

    private void OnDisable()
    {
        if (grabbable != null)
        {
            grabbable.WhenPointerEventRaised -= OnPointerEvent;
        }
    }

    private void OnPointerEvent(PointerEvent evt)
    {
        if (!hasStartedHideCountdown && evt.Type == PointerEventType.Select)
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

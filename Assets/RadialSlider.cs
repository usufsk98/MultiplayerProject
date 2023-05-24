using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RadialSlider : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] private Image fillImage;
    [SerializeField] private RectTransform handleImage;
    [Range(0, 1)] [SerializeField] private float value;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateSliderValue(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateSliderValue(eventData.position);
    }

    private void UpdateSliderValue(Vector2 inputPosition)
    {
        Vector2 localInputPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, inputPosition, null, out localInputPosition);

        float angle = Mathf.Atan2(localInputPosition.y, localInputPosition.x) * Mathf.Rad2Deg;
        if (angle < 0)
        {
            angle += 360;
        }

        value = angle / 360;
        fillImage.fillAmount = value;

        UpdateHandlePosition(angle);
    }

    private void UpdateHandlePosition(float angle)
    {
        float handleAngle = angle * Mathf.Deg2Rad;
        float radius = rectTransform.rect.width * 0.5f;
        Vector2 handlePosition = new Vector2(Mathf.Cos(handleAngle), Mathf.Sin(handleAngle)) * radius;
        handleImage.anchoredPosition = handlePosition;
    }
}
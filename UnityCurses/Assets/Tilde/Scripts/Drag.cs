using UnityEngine;
using UnityEngine.EventSystems;

namespace Tilde
{
    public class Drag : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        private Vector2 originalLocalPointerPosition;
        private Vector3 originalPanelLocalPosition;
        public RectTransform panelRectTransform;
        private RectTransform parentRectTransform;

        public void OnDrag(PointerEventData data)
        {
            if (panelRectTransform == null || parentRectTransform == null) return;

            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, data.position,
                data.pressEventCamera, out localPointerPosition))
            {
                Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
                panelRectTransform.localPosition = originalPanelLocalPosition + offsetToOriginal;
            }
        }

        public void OnPointerDown(PointerEventData data)
        {
            originalPanelLocalPosition = panelRectTransform.localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, data.position,
                data.pressEventCamera, out originalLocalPointerPosition);
        }

        private void Awake()
        {
            parentRectTransform = panelRectTransform.parent as RectTransform;
        }
    }
}
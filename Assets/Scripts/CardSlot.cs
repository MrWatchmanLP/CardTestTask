using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<Transform>().SetParent(GameManager.Instance.PanelTransform);
            eventData.pointerDrag.GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }
}

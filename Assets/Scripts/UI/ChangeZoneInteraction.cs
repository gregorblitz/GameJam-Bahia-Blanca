using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeZoneInteraction: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private int direction = 1; // 1 for next zone, -1 for previous zone
    [SerializeField] private SpriteRenderer highlightObject;
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Optional: Add visual feedback for hover state
        highlightObject.enabled = true;
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Optional: Remove visual feedback for hover state
        highlightObject.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Optional: Add visual feedback for click state
        Debug.Log("Mouse clicked ChangeZoneInteraction");
        ChangeZoneManager.Instance.ChangeZone(ChangeZoneManager.Instance.CurrentZoneIndex + direction);
    }

}
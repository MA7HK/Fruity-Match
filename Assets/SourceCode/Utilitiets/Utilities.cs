using UnityEngine;
using UnityEngine.EventSystems;

public static class Utilitites
{
    public static bool IsMouseOverUIObject()
    {
        // Create a pointer event data object
        PointerEventData eventData = new PointerEventData(EventSystem.current);

        // Set the position to the current mouse position
        eventData.position = Input.mousePosition;

        // Create a list to store all raycast results
        System.Collections.Generic.List<RaycastResult> results = new System.Collections.Generic.List<RaycastResult>();

        // Perform the raycast
        EventSystem.current.RaycastAll(eventData, results);

        // If there are any results, the mouse is over a UI object
        return results.Count > 0;
    }
}

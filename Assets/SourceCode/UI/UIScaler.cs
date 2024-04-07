using UnityEngine;
using UnityEngine.UI;

public class UIScaler : MonoBehaviour
{
    [SerializeField] private CanvasScaler canvasScaler;

    private void Start()
    {
        // Set the reference resolution (e.g., 1080x1920 for Full HD portrait)
        canvasScaler.referenceResolution = new Vector2(1080f, 1920f);

        // Calculate the screen aspect ratio
        float screenAspectRatio = (float)Screen.width / Screen.height;

        // Adjust the canvas scale based on the screen aspect ratio
        if (screenAspectRatio < 9f / 16f) // Wider screens (e.g., landscape)
        {
            canvasScaler.matchWidthOrHeight = 1f; // Match width
        }
        else // Taller screens (e.g., portrait)
        {
            canvasScaler.matchWidthOrHeight = 0f; // Match height
        }
    }
}

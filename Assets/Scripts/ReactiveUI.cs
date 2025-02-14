using UnityEngine;

public class ReactiveUI : MonoBehaviour
{

    public Canvas Canvas;
    RectTransform PanelSafeArea;
    private ScreenOrientation currentOrientation;
    private Rect currentSafeArea;

    // Start is called before the first frame update
    void Start()
    {
        PanelSafeArea = GetComponent<RectTransform>();

        currentOrientation = Screen.orientation;
        currentSafeArea = Screen.safeArea;

        ApplySafeArea();
    }

    private void ApplySafeArea()
    {
        if (PanelSafeArea == null)
        {
            return;
        }

        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Canvas.pixelRect.width;
        anchorMin.y /= Canvas.pixelRect.height;

        anchorMax.x /= Canvas.pixelRect.width;
        anchorMax.y /= Canvas.pixelRect.height;

        //assign values to panel
        PanelSafeArea.anchorMin = anchorMin;
        PanelSafeArea.anchorMax = anchorMax;
    }

    // Update is called once per frame
    void Update()
    {
        if ((currentOrientation != Screen.orientation) || (currentSafeArea != Screen.safeArea))
        {
            ApplySafeArea();
        }
    }
}

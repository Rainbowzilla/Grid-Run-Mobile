using UnityEngine;

public class OrientationDetector : MonoBehaviour
{
    [Header("Orientation Status")]
    [SerializeField] private bool _isPortrait;
    [SerializeField] private bool _isLandscape;

    [Header("Debug")]
    [SerializeField] private bool _logChanges = true;

    private bool _lastPortraitState;

    void Start()
    {
        UpdateOrientation();
    }

    void Update()
    {
        UpdateOrientation();
    }

    private void UpdateOrientation()
    {
        bool isPortrait = Screen.height > Screen.width;

        if (isPortrait != _lastPortraitState && _logChanges)
        {
            Debug.Log($"Orientation changed: {(isPortrait ? "Portrait" : "Landscape")} " +
                      $"(Width: {Screen.width}, Height: {Screen.height})");
        }

        _lastPortraitState = isPortrait;
        _isPortrait = isPortrait;
        _isLandscape = !isPortrait;
    }

    // Public getters for other scripts
    public bool IsPortrait => _isPortrait;
    public bool IsLandscape => _isLandscape;

    // Optional: Visualize in Scene view or Game view (attach a UI Text or use this)
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 50),
                  $"Portrait: {_isPortrait}\nLandscape: {_isLandscape}\n" +
                  $"Aspect: {(float)Screen.width / Screen.height:F2}");
    }
}
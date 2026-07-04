using UnityEngine;

public class BackgroundHandler : MonoBehaviour
{
    private BackgroundScroller[] _scrollers;

    private void Start()
    {
        _scrollers = FindObjectsByType<BackgroundScroller>();
    }

    public void ToggleScrollSpeed(bool value)
    {
        if (_scrollers == null || _scrollers.Length <= 0)
            return;

        foreach (BackgroundScroller scroller in _scrollers)
        {
            scroller.ToggleSpeed(value);
        }
    }
}

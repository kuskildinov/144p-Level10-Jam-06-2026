using UnityEngine;
using UnityEngine.UI;

public class CristalPanel : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    
    public void UpdateCristalCount()
    {
        _slider.value = GlovalVars.CristalCount;
    }

    public void SetMaxCristalCount()
    {
        _slider.maxValue = GlovalVars.CurrentMaxCristalCount;
    }
}

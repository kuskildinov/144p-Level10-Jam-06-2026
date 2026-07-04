using UnityEngine;
using UnityEngine.UI;

public class CristalPanel : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    
    public void UpdateCristalCount()
    {
        SetMaxCristalCount();
        _slider.value = GlobalVars.CristalCount;
    }

    public void SetMaxCristalCount()
    {
        _slider.maxValue = GlobalVars.CurrentMaxCristalCount;
    }
}

using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private Renderer _targetRenderer;
    [SerializeField] private float _scrollSpeed = 0.1f;

    private Material _material;
    private Vector2 _offset;
    private float _currentSpeed;

    private void Awake()
    {
        _material = _targetRenderer.material;
        ToggleSpeed(true);
    }

    private void Update()
    {
        _offset.x += _currentSpeed * Time.deltaTime;

        _material.SetTextureOffset("_BaseMap", _offset);
    }

    public void ToggleSpeed(bool value)
    {
        if (value)
        {
            _currentSpeed = _scrollSpeed;
        }
        else
        {
            _currentSpeed = 0f;
        }
    }
}

using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private Renderer _targetRenderer;
    [SerializeField] private float _scrollSpeed = 0.1f;

    private Material _material;
    private Vector2 _offset;

    private void Awake()
    {
        _material = _targetRenderer.material;
    }

    private void Update()
    {
        _offset.x += _scrollSpeed * Time.deltaTime;

        _material.SetTextureOffset("_BaseMap", _offset);
    }
}

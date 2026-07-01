using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private float _smoothTime = 0.15f;
    [Header("Screen Bounds")]
    [SerializeField] private float _minX = -8f;
    [SerializeField] private float _maxX = 8f;
    [SerializeField] private float _minY = -4f;
    [SerializeField] private float _maxY = 4f;

    private Player _player;
    private PlayerInputHandler _inputHandler;
    private Vector2 _moveDirection;
    private Vector2 _velocity;
    private Vector2 _currentVelocity;

    public void Initialize(Player player, PlayerInputHandler inputHandler)
    {
        _player = player;
        _inputHandler = inputHandler;

        SubscribeToEvents();
    }

    private void Update()
    {
        if (!_player.IsActive || !_player.IsAlive)
            return;

        Move();
    }

    private void Move()
    {
        Vector2 targetVelocity =
        _moveDirection * _moveSpeed;

        _currentVelocity = Vector2.SmoothDamp(
            _currentVelocity,
            targetVelocity,
            ref _velocity,
            _smoothTime);

        _player.transform.position +=
            (Vector3)(_currentVelocity * Time.deltaTime);

        ClampPosition();
    }

    private void ClampPosition()
    {
        Vector3 position = _player.transform.position;

        position.x = Mathf.Clamp(position.x, _minX, _maxX);
        position.y = Mathf.Clamp(position.y, _minY, _maxY);

        _player.transform.position = position;
    }

    #region >>> INPUT

    private void OnMoveInputChanged(Vector2 moveInput)
    {
        _moveDirection = moveInput.normalized;
    }

    private void OnDashInputChanged(bool value)
    {
        _player.TryDash(value);
    }

    #endregion
    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _inputHandler.MoveInput += OnMoveInputChanged;
        _inputHandler.DashInput += OnDashInputChanged;
    }

    private void UnsubscriteFromEvents()
    {
        _inputHandler.MoveInput -= OnMoveInputChanged;
        _inputHandler.DashInput -= OnDashInputChanged;
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscriteFromEvents();
    }
}

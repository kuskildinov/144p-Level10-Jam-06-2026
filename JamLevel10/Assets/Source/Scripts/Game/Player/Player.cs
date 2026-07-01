using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerRoot _root;
    private PlayerInputHandler _inputHandler;
    private PlayerMovment _movment;
    private PlayerVisual _visual;
    private PlayerAttack _attack;

    private bool _isActive;
    private bool _isAlive = true;

    public bool IsActive => _isActive;
    public bool IsAlive => _isAlive;

    public void Initialize(PlayerRoot root, PlayerInputHandler inputHandler)
    {
        _root = root;
        _inputHandler = inputHandler;
        _isActive = true;

        InitializeMovment();
        InitializeVisual();
        InitializeAttack();
    }

    #region >>> ACTIVATION

    public void ToggleActivation(bool value)
    {
        _isActive = value;
    }

    #endregion
    #region >>> MOVMENT

    private void InitializeMovment()
    {
        _movment = GetComponentInChildren<PlayerMovment>();
        if(_movment == null) { Debug.LogError("Error: Cant find PlayerMovment on Player"); return; }
        _movment.Initialize(this, _inputHandler);
    }

    public void TryDash(bool value)
    {       
        _visual.SetMiniMode(value);
    }

    #endregion
    #region >>> VISUAL

    private void InitializeVisual()
    {
        _visual = GetComponentInChildren<PlayerVisual>();
        if (_visual == null) { Debug.LogError("Error: Cant find PlayerVisual on Player"); return; }
        _visual.Initialize(this, _inputHandler);
    }

    #endregion
    #region >>> ATTACK

    private void InitializeAttack()
    {
        _attack = GetComponentInChildren<PlayerAttack>();
        if (_attack == null) { Debug.LogError("Error: Cant find PlayerAttack on Player"); return; }
        _attack.Initialize(this, _inputHandler);
    }

    #endregion
    #region >>> HEALTH

    public void TakeDamage()
    {

    }

    #endregion
}

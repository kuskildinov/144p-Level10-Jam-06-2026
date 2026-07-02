using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerRoot _root;
    private PlayerInputHandler _inputHandler;
    private PlayerMovment _movment;
    private PlayerVisual _visual;
    private PlayerAttack _attack;
    private PlayerHealth _health;

    private bool _isActive;
    private bool _isAlive = true;
    private bool _canTakeDamage = true;

    public bool IsActive => _isActive;
    public bool IsAlive => _isAlive;
    public int CurrentHP => _health.CurrentHP;

    public void Initialize(PlayerRoot root, PlayerInputHandler inputHandler)
    {
        _root = root;
        _inputHandler = inputHandler;
        _isActive = true;

        InitializeMovment();
        InitializeVisual();
        InitializeAttack();
        InitializeHealth();
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

    private void InitializeHealth()
    {
        _health = GetComponentInChildren<PlayerHealth>();
        if (_health == null) { Debug.LogError("Error: Cant find PlayerHealth on Player"); return; }
        _health.Initialize(this);
    }

    public void TakeDamage()
    {
        if (!_canTakeDamage)
            return;

        _visual.OnTakeDamage();
        _health.OnDamageTaked();
        _root.UpdateHealthPanel();
        StartCoroutine(TakeDamageInvisability());
    }

    public void TakeHealth()
    {
        _health.OnHealthTaked();
        _root.UpdateHealthPanel();
    }

    public void Dead()
    {

    }

    private IEnumerator TakeDamageInvisability()
    {
        _canTakeDamage = false;
        yield return new WaitForSecondsRealtime(0.5f);
        _canTakeDamage = true;
    }

    #endregion
    #region >>> CRISTAL

    private void TakeCristal()
    { 
    
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<AID>(out AID aid))
        {
            TakeHealth();
            aid.Collect();
        }
        if(other.TryGetComponent<Cristal>(out Cristal cristal))
        {
            TakeCristal();
            cristal.Collect();
        }
    }
}

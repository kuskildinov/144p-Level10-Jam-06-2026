using UnityEngine;

public class InputsRoot : CompositeRoot
{
    [SerializeField] private UnityEngine.InputSystem.PlayerInput _playerInput;

    public override void Compose()
    {
        // DisableAllInputs();
        SetPlayerMode();
    }

    public void SetPlayerMode()
    {
        if (!_playerInput.inputIsActive)
            _playerInput.ActivateInput();
        _playerInput.SwitchCurrentActionMap("Player");
    }

    public void DisableAllInputs()
    {
        _playerInput.DeactivateInput();
    }
}

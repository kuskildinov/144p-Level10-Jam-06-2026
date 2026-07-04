using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public event Action<Vector2> MoveInput;
    public event Action<bool> DashInput;
    public event Action<bool> AttackInput;
    public event Action EndInput;

    public void OnMove(InputValue value)
    {
        MoveInput?.Invoke(value.Get<Vector2>());
    }

    public void OnDash(InputValue value)
    {
        if (value.isPressed)
        {
            DashInput?.Invoke(true);
        }
        else
        {
            DashInput?.Invoke(false);
        }
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {          
            AttackInput?.Invoke(true);
        }
        else
        {           
            AttackInput?.Invoke(false);
        }
    }

    public void OnEnd(InputValue value)
    {
        if (value.isPressed)
        {
            EndInput?.Invoke();
        }
    }
}

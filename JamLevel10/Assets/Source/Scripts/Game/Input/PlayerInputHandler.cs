using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public event Action<Vector2> MoveInput;
    public event Action DashInput;
    public event Action<bool> AttackInput;

    public void OnMove(InputValue value)
    {
        MoveInput?.Invoke(value.Get<Vector2>());
    }

    public void OnDash(InputValue value)
    {
        if (value.isPressed)
        {
            DashInput?.Invoke();
        }
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Нажали");
            AttackInput?.Invoke(true);
        }
        else
        {
            Debug.Log("Отпустили");
            AttackInput?.Invoke(false);
        }
    }
}

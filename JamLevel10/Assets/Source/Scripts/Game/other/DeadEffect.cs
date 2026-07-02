using UnityEngine;

public class DeadEffect : MonoBehaviour
{
    private void Start()
    {
        Destroy(this.gameObject, 0.6f);
    }
}

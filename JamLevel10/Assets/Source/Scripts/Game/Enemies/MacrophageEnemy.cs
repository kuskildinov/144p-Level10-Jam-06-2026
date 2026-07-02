using UnityEngine;

public class MacrophageEnemy : Enemy
{
    private const string AnimatorRunParam = "Run";
    [Header("Visual")]
    [SerializeField] private Transform _visual;
    [SerializeField] private Animator _animtor;
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float lootSpeed = 5f;
    [SerializeField] private float lootSearchRadius = 5f;

    private Loot _targetLoot;
    private Loot _carriedLoot;

    private State _state;

    private enum State
    {
        MoveForward,
        SeekLoot,
        Escape
    }

    protected override void Awake()
    {
        base.Awake();

        _state = State.MoveForward;

        FaceLeft();
    }

    private void Update()
    {
        switch (_state)
        {
            case State.MoveForward:
                MoveForward();
                SearchLoot();
                break;

            case State.SeekLoot:
                MoveToLoot();
                break;

            case State.Escape:
                Escape();
                break;
        }
    }

    private void MoveForward()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (transform.position.x <= _leftDestroyX)
        {
            Destroy(gameObject);
        }
    }

    private void SearchLoot()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            lootSearchRadius);

        foreach (Collider hit in hits)
        {
            Loot loot = hit.GetComponent<Loot>();

            if (loot == null)
                continue;

            _targetLoot = loot;
            _state = State.SeekLoot;
            return;
        }
    }

    private void MoveToLoot()
    {
        if (_targetLoot == null)
        {
            _state = State.MoveForward;
            return;
        }

        _animtor.SetBool(AnimatorRunParam, true);

        transform.position = Vector3.MoveTowards(
            transform.position,
            _targetLoot.transform.position,
            lootSpeed * Time.deltaTime);

        if (Vector3.Distance(
                transform.position,
                _targetLoot.transform.position) < 0.3f)
        {
            PickLoot();
        }
    }

    private void PickLoot()
    {
        _carriedLoot = _targetLoot;

        _targetLoot.gameObject.SetActive(false);

        FaceRight();

        _state = State.Escape;
    }

    private void Escape()
    {
        transform.position += Vector3.right * lootSpeed * Time.deltaTime;

        if (transform.position.x >= _rightDestroyX)
        {
            Destroy(gameObject);
        }
    }

    private void FaceLeft()
    {
        _visual.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void FaceRight()
    {
        _visual.localRotation = Quaternion.Euler(0f, 180f, 0f);
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
    }

    protected override void Die()
    {
        if (_carriedLoot != null)
        {
            _carriedLoot.gameObject.SetActive(true);
            _carriedLoot.transform.position = transform.position;
        }

        base.Die();
    }
}
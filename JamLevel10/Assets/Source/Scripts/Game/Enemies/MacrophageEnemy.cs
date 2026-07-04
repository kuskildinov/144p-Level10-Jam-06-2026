using System.Collections;
using UnityEngine;

public class MacrophageEnemy : Enemy
{
    private const string AnimatorRunParam = "Run";
    [Header("Visual")]
    [SerializeField] private Transform _visual;
    [SerializeField] private Animator _animtor;
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private float _lootSpeed = 9f;
    [SerializeField] private float _escapeSpeed = 5f;
    [SerializeField] private float _lootSearchRadius = 5f;
    [SerializeField] private float _lootReactionDelay = 2f;
    [Header("Sounds")]
    [SerializeField] protected AudioSource _source;
    [SerializeField] private AudioClip _takeItemSound;
    [SerializeField] private AudioClip _dieSound;

    private Loot _targetLoot;
    private Loot _carriedLoot;
    private float _currentSpeed;
    private Coroutine _lootDelayRoutine;

    private State _state;

    private enum State
    {
        MoveForward,
        SeekLoot,
        Escape
    }

    public override void Initialize(EnemiesRoot root, Transform attackPoint)
    {
        base.Initialize(root, attackPoint);

        _state = State.MoveForward;
        FaceLeft();
    }

    private void Update()
    {
        switch (_state)
        {
            case State.MoveForward:
                _currentSpeed = _moveSpeed;
                MoveForward();
                SearchLoot();
                break;

            case State.SeekLoot:
                {
                    MoveToLoot();
                    break;
                }

            case State.Escape:
                _currentSpeed = _escapeSpeed;
                Escape();
                break;
        }       
    }

    private void MoveForward()
    {
       
        transform.position += Vector3.left * _currentSpeed * Time.deltaTime;

        if (transform.position.x <= _leftDestroyX || transform.position.y <= _bottomDestroyY || transform.position.y >= _topDestroyY)
        {
            _root.OnEnemyDead(this);
        }
    }

    private void SearchLoot()
    {
        if (_lootDelayRoutine != null)
            return;

        Collider[] hits = Physics.OverlapSphere(transform.position, _lootSearchRadius);

        foreach (Collider hit in hits)
        {
            Loot loot = hit.GetComponent<Loot>();

            if (loot == null || loot.IsBusy)
                continue;

            _lootDelayRoutine = StartCoroutine(StartSeekLoot(loot));
            return;
        }
    }

    private IEnumerator StartSeekLoot(Loot loot)
    {
        if (loot == null)
            yield break;

        if (loot.IsBusy)
            yield break;

        loot.IsBusy = true;
        _targetLoot = loot;
        _state = State.SeekLoot;
        _currentSpeed = 0f;
        yield return new WaitForSeconds(_lootReactionDelay);
        _currentSpeed = _lootSpeed;
        _lootDelayRoutine = null;
    }

    private void MoveToLoot()
    {
        if (_targetLoot == null)
        {
            _state = State.MoveForward;
            _lootDelayRoutine = null;
            return;
        }      
        
        _animtor.SetBool(AnimatorRunParam, true);

        transform.position = Vector3.MoveTowards(
            transform.position,
            _targetLoot.transform.position,
            _currentSpeed * Time.deltaTime);
               

        if (Vector3.Distance(
                transform.position,
                _targetLoot.transform.position) < 0.1f)
        {
            if(_targetLoot == null)
            {
                _state = State.MoveForward;
                return;
            }
            PickLoot();
        }
    }

    private void PickLoot()
    {
        _carriedLoot = _targetLoot;

        _targetLoot.gameObject.SetActive(false);

        PlayTakeItemSound();
        FaceRight();

        _state = State.Escape;
    }

    private void Escape()
    {
        transform.position += Vector3.right * _currentSpeed * Time.deltaTime;

        if (transform.position.x >= _rightDestroyX)
        {
            _root.OnEnemyDead(this);
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

    protected override void Die()
    {
        if (_carriedLoot != null)
        {
            _carriedLoot.gameObject.SetActive(true);
            _carriedLoot.transform.position = transform.position;
            _carriedLoot.IsBusy = false;
        }

        PlayDieSound();
        base.Die();
    }

    #region >>> SOUNDS

    private void PlayTakeItemSound()
    {       
        _source.PlayOneShot(_takeItemSound);
    }

    private void PlayDieSound()
    {
        _source.PlayOneShot(_dieSound);
    }

    #endregion
}
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BossLaserAttack : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private LineRenderer _line;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Transform _crosshairPrefab;

    [Header("Timings")]
    [SerializeField] private float _trackTime = 1.5f;
    [SerializeField] private float _chargeTime = 1f;
    [SerializeField] private float _fireTime = 0.5f;

    [Header("Width")]
    [SerializeField] private float _aimWidth = 0.05f;
    [SerializeField] private float _fireWidth = 0.8f;

    [Header("Colors")]
    [SerializeField] private Color aimColor = new(0.65f, 0f, 1f);
    [SerializeField] private Color chargeColor = new(1f, 0f, 0.6f);

    [Header("Sounds")]
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _prepareSound;
    [SerializeField] private AudioClip _laserSound;

    [SerializeField] private AnimationCurve _widthCurve;

    private Boss _boss;
    private Transform _crosshair;

    private Tween _widthTween;

    private bool _executing;

    public void Initialize(Boss boss)
    {
        _boss = boss;
        ResetLaser();
    }

    public IEnumerator Execute()
    {
        if (_executing)
            yield break;

        yield return new WaitForSecondsRealtime(3f);

        _executing = true;

        ResetLaser();

        float timer = 0f;
        Vector3 target = Vector3.zero;

        while (timer < _trackTime)
        {
            if (_boss == null || _boss.Player == null)
            {
                FinishAttack();
                yield break;
            }

            target = _boss.Player.transform.position;

            UpdateCrosshair(target);          
            timer += Time.deltaTime;
            yield return null;
        }

        yield return Charge(target);

        if (_crosshair != null)
            Destroy(_crosshair.gameObject);

        yield return Fire(target);

        FinishAttack();
    }

    private IEnumerator Charge(Vector3 target)
    {
        _boss.ToggleLookAtPlayer(false);

        float timer = 0f;

        _source.PlayOneShot(_prepareSound);
        while (timer < _chargeTime)
        {
            float pulse = Mathf.Lerp(1f, 1.3f, Mathf.PingPong(timer * 4f, 1f));

            if (_crosshair != null)
                _crosshair.localScale = Vector3.one * pulse;

            timer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Fire(Vector3 target)
    {
        Vector3 start = _firePoint.position;
        Vector3 dir = (target - start).normalized;

        const float beamLength = 100f;

        _line.enabled = true;

        _source.PlayOneShot(_laserSound);
        _line.widthCurve = _widthCurve;
        _line.startColor = chargeColor;
        _line.endColor = chargeColor;
        _line.widthMultiplier = 0.05f;

        _widthTween?.Kill();

        _widthTween = DOTween.To(
            () => _line.widthMultiplier,
            x => _line.widthMultiplier = x,
            _fireWidth,
            0.1f);

        bool damageDone = false;

        float timer = 0f;

        while (timer < _fireTime)
        {
            start = _firePoint.position;
            dir = (target - start).normalized;

            Vector3 end = start + dir * beamLength;

            _line.SetPosition(0, start);
            _line.SetPosition(1, end);

            if (!damageDone &&
                Physics.Raycast(start, dir, out RaycastHit hit, beamLength))
            {
                Player player = hit.collider.GetComponentInParent<Player>();

                if (player != null)
                {
                    damageDone = true;
                    player.TakeDamage();
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }

        _widthTween?.Kill();

        _widthTween = DOTween.To(
            () => _line.widthMultiplier,
            x => _line.widthMultiplier = x,
            0f,
            0.15f);

        yield return new WaitForSeconds(0.15f);

        _line.enabled = false;
    }
  
    private void UpdateCrosshair(Vector3 target)
    {
        if (_crosshair == null)
            _crosshair = Instantiate(_crosshairPrefab);

        _crosshair.position = target;
    }

    private void ResetLaser()
    {
        _widthTween?.Kill();

        _line.enabled = false;

        _line.widthCurve = AnimationCurve.Linear(0, 1, 1, 1);

        _line.startWidth = _aimWidth;
        _line.endWidth = _aimWidth;

        _line.widthMultiplier = 1f;

        _line.startColor = aimColor;
        _line.endColor = aimColor;
    }

    private void FinishAttack()
    {
        _executing = false;

        _widthTween?.Kill();

        if (_crosshair != null)
            Destroy(_crosshair.gameObject);

        _line.enabled = false;

        _boss.ToggleLookAtPlayer(true);
    }

    private void OnDisable()
    {
        FinishAttack();
    }
}
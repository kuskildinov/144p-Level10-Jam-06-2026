using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BossLaserAttack : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Transform _crosshairPrefab;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private AnimationCurve _widthCurve;
    [SerializeField] private float _trackTime = 1.5f;
    [SerializeField] private float _chargeTime = 1f;
    [SerializeField] private float _fireTime = 0.5f;
    [SerializeField] private float _aimWidth = 0.05f;
    [SerializeField] private float _chargeWidth = 0.15f;
    [SerializeField] private float _fireWidth = 0.8f;

    private Boss _boss;
    private Transform _crosshairInstance;

    private Tween _widthTween;
    private Tween _colorTween;

    [SerializeField] private Color aimColor = new Color(0.65f, 0f, 1f);   // purple
    [SerializeField] private Color chargeColor = new Color(1f, 0f, 0.6f); // red-purple

    public void Initialize(Boss boss)
    {
        _boss = boss;
    }

    public IEnumerator Execute()
    {
        Vector3 lockedTarget = Vector3.zero;

        _line.enabled = false;
        SetAimVisual();               
        float t = 0f;

        while (t < _trackTime)
        {
            Vector3 target = _boss.Player.transform.position;

            UpdateCrosshair(target);
            DrawLaser(target);

            t += Time.deltaTime;
            yield return null;
        }

        lockedTarget = _boss.Player.transform.position;

        yield return StartCoroutine(Charge(lockedTarget));

        if (_crosshairInstance != null)
            Destroy(_crosshairInstance.gameObject);

        Vector3 start = _firePoint.position;
        Vector3 direction = (lockedTarget - start).normalized;              
        Vector3 endPoint = start + direction * 100f;

        yield return StartCoroutine(FireLaser(endPoint));
    }
       
    private void SetAimVisual()
    {
        KillTweens();

        _line.startColor = aimColor;
        _line.endColor = aimColor;

        _line.startWidth = _aimWidth;
        _line.endWidth = _aimWidth;
    }
       
    private IEnumerator Charge(Vector3 lockedTarget)
    {
        _boss.ToggleLookAtPlayer(false);

        KillTweens();

        float t = 0f;

        while (t < _chargeTime)
        {
            float pulse = Mathf.PingPong(Time.time * 10f, 1f);

            _line.startWidth = Mathf.Lerp(0.05f, 0.2f, pulse);
            _line.endWidth = _line.startWidth;

            _line.startColor = Color.Lerp(aimColor, chargeColor, pulse);
            _line.endColor = _line.startColor;

            DrawLaser(lockedTarget);

            t += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FireLaser(Vector3 target)
    {
        KillTweens();

        _line.enabled = true;

        Vector3 start = _firePoint.position;
        Vector3 direction = (target - start).normalized;

        const float beamLength = 100f;

        _line.widthCurve = _widthCurve;
               
        _line.widthMultiplier = 0.05f;

        _line.startColor = chargeColor;
        _line.endColor = chargeColor;
               
        Tween widthTween = DOTween.To(
            () => _line.widthMultiplier,
            x => _line.widthMultiplier = x,
            _fireWidth,
            0.1f)
            .SetEase(Ease.OutExpo);

        bool damageDone = false;

        float timer = 0f;

        while (timer < _fireTime)
        {
            start = _firePoint.position;
            direction = (target - start).normalized;

            Vector3 endPoint = start + direction * beamLength;

            if (Physics.Raycast(start, direction, out RaycastHit hit, beamLength))
            {                
                if (!damageDone)
                {
                    Player player = hit.collider.GetComponentInParent<Player>();

                    if (player != null)
                    {
                        damageDone = true;
                        player.TakeDamage();
                    }
                }
            }

            _line.SetPosition(0, start);
            _line.SetPosition(1, endPoint);
           
            timer += Time.deltaTime;
            yield return null;
        }

        yield return widthTween.WaitForCompletion();

        // Исчезновение
        yield return DOTween.To(
            () => _line.widthMultiplier,
            x => _line.widthMultiplier = x,
            0f,
            0.15f).WaitForCompletion();

        _line.enabled = false;
    }

    private void DrawLaser(Vector3 target)
    {
        _line.SetPosition(0, _firePoint.position);
        _line.SetPosition(1, target);
    }
       
    private void UpdateCrosshair(Vector3 target)
    {
        if (_crosshairInstance == null)
        {
            _crosshairInstance = Instantiate(_crosshairPrefab);
        }

        _crosshairInstance.position = target;
    }

    private void KillTweens()
    {
        _widthTween?.Kill();
        _colorTween?.Kill();
        DOTween.Kill(_line);
    }
}
using UnityEngine;

public class SoundsRoot : CompositeRoot
{
    public static SoundsRoot Instance;

    [Header("Player")]
    [SerializeField] private AudioSource _playerSource;
    [SerializeField] private AudioClip _playerAttackSound;
    [SerializeField] private AudioClip _playerDeathSound;
    [SerializeField] private AudioClip _playerTakeHpSound;
    [SerializeField] private AudioClip _playerTakeExpSound;
    [SerializeField] private AudioClip _playerLevelUpSound;
    [Header("Enemy")]
    [SerializeField] private AudioSource _enemySource;
    [SerializeField] private AudioClip _enemyExplotionSound;
    [Header("Ui")]
    [SerializeField] private AudioSource _uiSource;
    [SerializeField] private AudioClip _clickSound;
    [Header("Background")]
    [SerializeField] private AudioSource _backgroundSource;
    [SerializeField] private AudioClip _mainSound;
    [SerializeField] private AudioClip _bossSound;

    public override void Compose()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        Instance = this;
    }

    #region >>> PLAYER

    public void PlayAttackSound()
    {
        _playerSource.PlayOneShot(_playerAttackSound);
    }

    public void PlayDeadSound()
    {
        _playerSource.PlayOneShot(_playerDeathSound);
    }

    public void PlayTakeHpSound()
    {
        _playerSource.PlayOneShot(_playerTakeHpSound);
    }

    public void PlayTakeExpSound()
    {
        _playerSource.PlayOneShot(_playerTakeExpSound);
    }

    public void PlayLevelUpSound()
    {
        _playerSource.PlayOneShot(_playerLevelUpSound);
    }

    #endregion
    #region >>> ENEMIES

    public void PlayExpotionSound()
    {
        _enemySource.PlayOneShot(_enemyExplotionSound);
    }

    #endregion
    #region >>> Ui

    public void PlayClickSound()
    {
        _uiSource.PlayOneShot(_clickSound);
    }

    #endregion
    #region >>> BACKGROUND

    public void StartBossSound()
    {
        _backgroundSource.clip = _bossSound;
    }

    #endregion

}

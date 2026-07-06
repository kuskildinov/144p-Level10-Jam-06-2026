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
    [Header("Phrases")]
    [SerializeField] private AudioSource _phrasesSource;
    [SerializeField] private AudioClip _startPhrase_1;
    [SerializeField] private AudioClip _startPhrase_2;
    [SerializeField] private AudioClip _bossPhrase;
    [SerializeField] private AudioClip _deadPhrase_1;
    [SerializeField] private AudioClip _deadPhrase_2;
    [SerializeField] private AudioClip _deadPhrase_3;
    [SerializeField] private AudioClip _deadPhrase_4;
    [SerializeField] private AudioClip _levelUpPhrase;

    private bool _abilityPhraseSaid = false;

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
        _backgroundSource.Play();
    }

    #endregion
    #region >>> PHRASES

    public void PlayStartPhrase_1()
    {
        _phrasesSource.PlayOneShot(_startPhrase_1);
    }

    public void PlayStartPhrase_2()
    {
        _phrasesSource.PlayOneShot(_startPhrase_2);
    }

    public void PlayBossPhrase()
    {
        _phrasesSource.PlayOneShot(_bossPhrase);
    }

    public void PlayDeadPhrase()
    {
        int index = Random.Range(0,4);
        switch (index)
        {
            case 0:
                {
                    _phrasesSource.PlayOneShot(_deadPhrase_1);
                    break;
                }
            case 1:
                {
                    _phrasesSource.PlayOneShot(_deadPhrase_2);
                    break;
                }
            case 2:
                {
                    _phrasesSource.PlayOneShot(_deadPhrase_3);
                    break;
                }
            case 3:
                {
                    _phrasesSource.PlayOneShot(_deadPhrase_4);
                    break;
                }
        }
    }

    public void PlayLevelUpPhrase()
    {
        if (_abilityPhraseSaid)
            return;
        _abilityPhraseSaid = true;
        _phrasesSource.PlayOneShot(_levelUpPhrase);
    }
    #endregion

}

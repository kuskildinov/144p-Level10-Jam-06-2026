using DG.Tweening;
using System.Collections;
using UnityEngine;

public class FinalCutScene : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform _playerStandPoint;
    [Header("Queen")]
    [SerializeField] private Transform _queenSpawnPoint;
    [SerializeField] private Transform _queenStandPoint;
    [SerializeField] private GameObject _queen;
    [Header("UI")]
    [SerializeField] private GameObject _pressFPanel;
    [SerializeField] private GameObject _finalPanel;
    [SerializeField] private CanvasVideoPlayer _videoPlayer;

    private LevelRoot _root;
    private Player _player;
    private PlayerPanel _playerPanel;
    private BackgroundHandler _backgroundHandler;
    private PlayerInputHandler _inputHandler;
    private Tween _playerMoveTween;
    private Tween _queenMoveTween;
    private bool _cutSceneActive;

    public void Initialize(LevelRoot root)
    {
        _root = root;
        _queen.transform.position = _queenSpawnPoint.position;
        _videoPlayer.Initialize(this);
        _queen.SetActive(false);
        TogglePressFPanel(false);
        GetOtherLinks();
        SubscribeToEvents();
    }

    public void StartCutScene()
    {
        StartCoroutine(StartCutSceneRoutine());
    }

    private void GetOtherLinks()
    {
        _player = FindAnyObjectByType<Player>();
        if (_player == null) { Debug.LogError("Error: Cant find Player on scene"); return; }
        _backgroundHandler = FindAnyObjectByType<BackgroundHandler>();
        if (_backgroundHandler == null) { Debug.LogError("Error: Cant find BackgroundHandler on scene"); return; }
        _inputHandler = FindAnyObjectByType<PlayerInputHandler>();
        if (_inputHandler == null){Debug.LogError("Error: Cant find PlayerInputHandler on scene!");return;}
        _playerPanel = FindAnyObjectByType<PlayerPanel>();
        if (_playerPanel == null) { Debug.LogError("Error: Cant find PlayerPanel on scene!"); return; }
    }

    private void MovePlayerToStandPoint()
    {
        _player.ToggleActivation(false);
        _playerMoveTween = _player.transform.DOMove(_playerStandPoint.position, 3)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
               
            });
    }

    private void ShowAndMoveQueen()
    {
        _cutSceneActive = true;
        _queen.gameObject.SetActive(true);
        _queenMoveTween = _queen.transform.DOMove(_queenStandPoint.position, 3)
           .SetEase(Ease.InOutSine)
           .OnComplete(() =>
           {

           });

    }

    private void TogglePressFPanel(bool value)
    {
        _pressFPanel.gameObject.SetActive(value);
    }

    private void HidePlayerUi()
    {
        _playerPanel.ToggleVisibility(false);
    }

    private IEnumerator StartCutSceneRoutine()
    {
        yield return new WaitForSecondsRealtime(10f);
        MovePlayerToStandPoint();
        HidePlayerUi();
        yield return new WaitForSecondsRealtime(0.1f);
        ShowAndMoveQueen();
        yield return new WaitForSecondsRealtime(1f);
        TogglePressFPanel(true);
    }

    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _inputHandler.EndInput += OnEndButtonClicked;
    }

    private void UnsubscribeToEvents()
    {
        _inputHandler.EndInput -= OnEndButtonClicked;
    }

    public void OnEndButtonClicked()
    {
        if (!_cutSceneActive)
            return;
        TogglePressFPanel(false);
        _finalPanel.gameObject.SetActive(true);
        _videoPlayer.Play();
    }

    public void OnVideoEnd()
    {
        _root.BackToMainMenu();
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}

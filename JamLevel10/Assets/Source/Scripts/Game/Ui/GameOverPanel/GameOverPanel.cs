using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;

    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _backToMenuButton;

    private LevelRoot _root;

    public void Initialize(LevelRoot root)
    {
        _root = root;
        SubscribeToEvents();
    }

    #region >>> OPEN CLOSE
    public void Open()
    {
        _panel.gameObject.SetActive(true);
    }

    public void Close()
    {
        _panel.gameObject.SetActive(false);
    }

    #endregion
    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
        _backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);
    }

    private void UnSubscribeToEvents()
    {
        _restartButton.onClick.RemoveAllListeners();
        _backToMenuButton.onClick.RemoveAllListeners();
    }

    private void OnRestartButtonClicked()
    {
        _root.OnRestartButtonClicked();
    }

    private void OnBackToMenuButtonClicked()
    {
        _root.OnBackToMenuButtonClicked();
    }

    #endregion

    private void OnDestroy()
    {
        UnSubscribeToEvents();
    }
}

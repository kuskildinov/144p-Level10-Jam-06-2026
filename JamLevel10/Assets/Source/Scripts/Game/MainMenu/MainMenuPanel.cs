using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _continueGameButton;
    [SerializeField] private Button _exitGameButton;

    private MainMenuRoot _root;

    public void Initialize(MainMenuRoot root)
    {
        _root = root;

        SubscribeToEvents();
    }

    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _newGameButton.onClick.AddListener(OnNewGameButtonClicked);
        _continueGameButton.onClick.AddListener(OnContinueGameButtonClicked);
        _exitGameButton.onClick.AddListener(OnExitGameButtonClicked);
    }

    private void UnsubscribeToEvents()
    {
        _newGameButton.onClick.RemoveAllListeners();
        _continueGameButton.onClick.RemoveAllListeners();
        _exitGameButton.onClick.RemoveAllListeners();
    }

    private void OnNewGameButtonClicked()
    {
        _root.OnNewGameButtonClicked();
    }

    private void OnContinueGameButtonClicked()
    {
        _root.OnContinueGameButtonClicked();
    }

    private void OnExitGameButtonClicked()
    {
        _root.OnExitGameButtonClicked();
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}

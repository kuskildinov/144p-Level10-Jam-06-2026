using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuRoot : CompositeRoot
{
    [SerializeField] private string _firstLevelSceneName;
    private MainMenuPanel _mainMenuPanel;
    private BlackFade _blackFade;

    public override void Compose()
    {
        InitializeMainMenuPanel();

        GetOtherLinks();
        HideBlackFade();
    }

    private void GetOtherLinks()
    {
        _blackFade = FindAnyObjectByType<BlackFade>();
        if (_blackFade == null) { Debug.LogError("Error: Cant find BlackFade on scene"); return; }
    }

    #region >>> MAIN MENU PANEL
    private void InitializeMainMenuPanel()
    {
        _mainMenuPanel = FindAnyObjectByType<MainMenuPanel>();
        if (_mainMenuPanel == null) { Debug.LogError("Error: Cant find MainMenuPanel on scene");return; }
        _mainMenuPanel.Initialize(this);
    }

    public void OnNewGameButtonClicked()
    {
        TryStartNewGame();
    }

    public void OnContinueGameButtonClicked()
    {
        TryContinueGame();
    }

    public void OnExitGameButtonClicked()
    {
        ExitGame();
    }

    #endregion
    #region >>> BLACK FADE

    private void ShowBlackFade()
    {
        _blackFade.FadeIn();
    }

    private void HideBlackFade()
    {
        _blackFade.FadeOut();
    }

    #endregion

    private void TryStartNewGame()
    {
        LoadGameScene();
    }

    private void TryContinueGame()
    {
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        StartCoroutine(LoadGameSceneRoutine());
    }

    private IEnumerator LoadGameSceneRoutine()
    {
        _blackFade.FadeIn(() =>
        {           
            SceneManager.LoadScene(_firstLevelSceneName);
        });
        yield return null;
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}

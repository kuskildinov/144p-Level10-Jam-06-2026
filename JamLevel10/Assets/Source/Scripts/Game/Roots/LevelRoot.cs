using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRoot : CompositeRoot
{
    [SerializeField] private FinalCutScene _finalCutScene;
    [SerializeField] private UpgradePanel _upgradePanel;
    private GameOverPanel _gameOverPanel;
    private BlackFade _blackFade;
    private DialogsRoot _dialogsRoot;
    private ILevelFlow _currentFlow;
    

    public override void Compose()
    {
        InitializeGameOverPanel();
        InitializeFinalCutScene();
        InitializeGameFlow();
        GetOtherLinks();

        HideBlackFade();
    }

    #region >>> other links

    private void GetOtherLinks()
    {
        _dialogsRoot = FindAnyObjectByType<DialogsRoot>();
        if (_dialogsRoot == null) { Debug.Log("Error: Cant find DialogsRoot on scene"); return; }
         _blackFade = FindAnyObjectByType<BlackFade>();
        if (_blackFade == null) { Debug.LogError("Error: Cant find BlackFade on scene"); return; }
    }


    #endregion
    #region >>> DIALOGS

    public void ShowDialogByIndex(int index, Action onComplete)
    {
        _dialogsRoot.TryStartDialog(index, onComplete);
    }

    public void HideDialog()
    {
        _dialogsRoot.EndDialog();
    }

    #endregion
    #region >>> BLACK FADE

    private void ShowBlackFade()
    {
        _blackFade.FadeIn();
    }

    public void HideBlackFade()
    {
        _blackFade.FadeOut();
    }
    
    #endregion
    #region >>> GAME OVER

    private void InitializeGameOverPanel()
    {
        _gameOverPanel = FindAnyObjectByType<GameOverPanel>();
        if (_gameOverPanel == null) { Debug.LogError("Error: Cant find GameOverPanel on scene"); return; }
        _gameOverPanel.Initialize(this);
    }

    public void OnPlayerDead()
    {       
        ShowGameOverPanel();
    }

    private void ShowGameOverPanel()
    {
        _gameOverPanel.Open();
    }


    public void OnRestartButtonClicked()
    {
        GlobalVars.NeedSkipTips = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnBackToMenuButtonClicked()
    {
        SceneManager.LoadScene(1);
    }

    #endregion
    #region >>> FINAL CUT SCENE

    private void InitializeFinalCutScene()
    {
        if (_finalCutScene == null)
            return;

        _finalCutScene.Initialize(this);
    }

    public void TryEndCutScene()
    {

    }

    #endregion
    #region >>> GAME FLOW

    private void InitializeGameFlow()
    {
        _currentFlow = GetComponent<ILevelFlow>();
        if (_currentFlow == null) { Debug.Log("Error: Cant find ILevelFlow on scene");return; }
        _currentFlow.Initialzie(this);
    }


    #endregion
    #region >>> ABILITY PANEL

    public void OpenAbilityPanel()
    {
        _upgradePanel.Open();
    }

    #endregion
}

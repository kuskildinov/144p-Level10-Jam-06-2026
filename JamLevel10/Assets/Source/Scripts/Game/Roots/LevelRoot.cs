using System;
using UnityEngine;

public class LevelRoot : CompositeRoot
{    
    private GameOverPanel _gameOverPanel;
    private BlackFade _blackFade;
    private DialogsRoot _dialogsRoot;

    public override void Compose()
    {
        InitializeGameOverPanel();
        GetOtherLinks();

        HideBlackFade();
        //TEST
        Time.timeScale = 1f;
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

    private void ShowDialogByIndex(int index, Action onComplete)
    {
        _dialogsRoot.TryStartDialog(index, onComplete);
    }

    private void HideDialog()
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
        //TEST
        Time.timeScale = 0f;
        ShowGameOverPanel();
    }

    private void ShowGameOverPanel()
    {
        _gameOverPanel.Open();
    }


    public void OnRestartButtonClicked()
    {

    }

    public void OnBackToMenuButtonClicked()
    {

    }

    #endregion
}

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRoot : CompositeRoot
{
    [SerializeField] private FinalCutScene _finalCutScene;
    [SerializeField] private string _finalCutSceneName;
    [SerializeField] private UpgradePanel _upgradePanel;
    private GameOverPanel _gameOverPanel;
    private BlackFade _blackFade;
    private DialogsRoot _dialogsRoot;
    [Header("Levels")]
    [SerializeField] private Level_1_Flow _Level_1_Flow;
    [SerializeField] private Level_2_Flow _Level_2_Flow;
    [SerializeField] private Level_3_Flow _Level_3_Flow;
    [SerializeField] private Level_bossFlow _Level_boss_Flow;

    private ILevelFlow _currentLevelFlow;

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
        SoundsRoot.Instance.PlayDeadPhrase();
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

    public void StartEndCutScene()
    {
        _finalCutScene.StartCutScene();
    }


    public void TryEndCutScene()
    {
        SceneManager.LoadScene(_finalCutSceneName);
    }

    #endregion
    #region >>> GAME FLOW

    public void OnLevelChanged()
    {
        if(GlobalVars.Level == 3) //3
        {
            Debug.Log("переход на уровень 2");
            MoveToLevel_2();
        }
        else if(GlobalVars.Level == 4) //4
        {
            Debug.Log("переход на уровень 3");
            MoveToLevel_3();
        }
        else if (GlobalVars.Level == 5) //5
        {
            Debug.Log("переход на уровень босс");
            MoveToLevel_boss();
        }
    }

    private void InitializeGameFlow()
    {
        _currentLevelFlow = _Level_1_Flow;
        _currentLevelFlow.Initialzie(this);
    }

    public void MoveToLevel_2()
    {
        _Level_1_Flow.enabled = false;

        _currentLevelFlow = _Level_2_Flow;
        _currentLevelFlow.Initialzie(this);
    }

    public void MoveToLevel_3()
    {
        _Level_2_Flow.enabled = false;

        _currentLevelFlow = _Level_3_Flow;
        _currentLevelFlow.Initialzie(this);
    }

    public void MoveToLevel_boss()
    {
        _Level_3_Flow.enabled = false;

        _currentLevelFlow = _Level_boss_Flow;
        _currentLevelFlow.Initialzie(this);
    }

    #endregion
    #region >>> ABILITY PANEL

    public void OpenAbilityPanel()
    {
        SoundsRoot.Instance.PlayLevelUpPhrase();
        _upgradePanel.Open();
    }

    #endregion

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(1);
    }
}

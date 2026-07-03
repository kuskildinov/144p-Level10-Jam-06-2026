using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogsRoot : CompositeRoot
{
    [SerializeField] private List<DialogPhrase> _currentLevelPhrases;

    private DialogPanel _dialogPanel;
    private PlayerRoot _playeRoot;
    private GameStatesRoot _gameStatesRoot;
    private InputsRoot _inputRoot;
       
    public override void Compose()
    {
        InitializeDialogPanel();      
        GetOtherLinks();      
    }

    #region >>> INITIALIZE    
    private void InitializeDialogPanel()
    {
        _dialogPanel = FindAnyObjectByType<DialogPanel>();
        if (_dialogPanel == null)
        {
            Debug.LogError("Cant Find DialogPanel on scene");
            return;
        }
        _dialogPanel.Initialize(this);
    }

    private void GetOtherLinks()
    {
        _gameStatesRoot = FindAnyObjectByType<GameStatesRoot>();
        if (_gameStatesRoot == null) Debug.LogError("Error: Cant find GameStatesRoot on scene");
        _playeRoot = FindAnyObjectByType<PlayerRoot>();
        if (_playeRoot == null) { Debug.LogError("Cant find PlayerRoot on scene"); return; }
        _inputRoot = FindAnyObjectByType<InputsRoot>();
        if (_inputRoot == null) { Debug.LogError("Cant find InputsRoot on scene"); return; }
    }

    #endregion 
    #region >>> DIALOG PANEL

    public void TryStartDialog(int index,Action onComplete)
    {
        _dialogPanel.Open();      
              
        DialogPhrase phrase = _currentLevelPhrases[index];
        StartDialog(phrase, onComplete);
    }

    public void EndDialog()
    {        
        _currentLevelPhrases = null;
        _dialogPanel.Close();         
    }

    private void StartDialog(DialogPhrase phrase, Action onComplete)
    {
        _dialogPanel.ShowDialog(phrase, onComplete);
    }   
    #endregion
}

[Serializable]
public struct DialogPhrase
{
    [TextArea] public string Phrase;
    public Color Color;   
}
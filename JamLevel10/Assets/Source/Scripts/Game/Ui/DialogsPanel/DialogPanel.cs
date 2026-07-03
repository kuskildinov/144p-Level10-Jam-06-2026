using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogPanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextWriter _textWriter;

    private DialogsRoot _root;
    private bool _isOpen;

    public bool IsOpen => _isOpen;

    public void Initialize(DialogsRoot root)
    {
        _root = root;
    }

    #region >>> OPEN CLOSE

    public void Open()
    {
        _isOpen = true;
        _panel.gameObject.SetActive(true);
    }

    public void Close()
    {
        _isOpen = false;
        _panel.gameObject.SetActive(false);
    }

    #endregion
    #region >>> DIALOG

    public void ShowDialog(DialogPhrase phrase, Action onComplete)
    {
        _textWriter.ChangeTextAndTyping(phrase, onComplete);
    }

    public void TrySkip(Action onComplete)
    {
        if (_textWriter.CurrentText == _textWriter.FullText)
        {
            onComplete?.Invoke();
        }
        else
        {
            _textWriter.SkipTyping();
        }
    }

    #endregion
}

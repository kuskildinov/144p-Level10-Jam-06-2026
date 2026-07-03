using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TextWriter : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _typingSpeed = 0.05f;
    [SerializeField] private float _timeDeleyAfterTyping = 2f;
    [SerializeField] private bool skipOnClick = true;

    private string _fullText;
    private string _currentText = "";
    private Coroutine _typingCoroutine;
    private bool _isTyping = false;

    public bool IsTyping => _isTyping;
    public string FullText => _fullText;
    public string CurrentText => _text.text;

    public event Action TypingStart;
    public event Action TypingComplete;

    public void ChangeTextAndTyping(DialogPhrase dialog, Action onComplete)
    {
        _fullText = dialog.Phrase;
        _text.color = dialog.Color;

        StartTyping(onComplete);
    }

    private void StartTyping(Action onComplete)
    {
        if (_isTyping)
            StopTyping();

        ClearText();
        TypingStart?.Invoke();
        _typingCoroutine = StartCoroutine(TypeText(onComplete));
    }

    public void StopTyping()
    {
        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _isTyping = false;
        ClearText();
    }

    public void SkipTyping()
    {
        if (!_isTyping) return;

        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        ShowFullText();
        _isTyping = false;
    }

    public void ShowFullText()
    {
        StartCoroutine(ShowAllTextRoutine());
    }

    private void ClearText()
    {
        _currentText = "";
        UpdateTextDisplay(_currentText);
    }

    private void PlaySoundByType()
    {

    }

    private IEnumerator TypeText(Action onComplete)
    {
        _isTyping = true;

        for (int i = 0; i <= _fullText.Length; i++)
        {
            PlaySoundByType();
            _currentText = _fullText.Substring(0, i);
            UpdateTextDisplay(_currentText);

            yield return new WaitForSeconds(_typingSpeed);
        }
        _isTyping = false;
        yield return new WaitForSecondsRealtime(_timeDeleyAfterTyping);
        onComplete?.Invoke();
    }

    private IEnumerator ShowAllTextRoutine()
    {
        _currentText = _fullText;
        UpdateTextDisplay(_currentText);
        yield return new WaitForSecondsRealtime(_timeDeleyAfterTyping);
        yield return null;
    }

    private void UpdateTextDisplay(string text)
    {
        _text.text = text;
    }
}

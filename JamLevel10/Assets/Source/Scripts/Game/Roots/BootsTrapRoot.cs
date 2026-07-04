using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootsTrapRoot : CompositeRoot
{
    [SerializeField] private string _mainMenuSceneName;
    private BlackFade _blackFade;

    public override void Compose()
    {
        GetOtherLinks();      
    }

    private void Start()
    {
        StartCoroutine(ShowLogoRoutine());
    }

    private void GetOtherLinks()
    {
        _blackFade = FindAnyObjectByType<BlackFade>();
        if (_blackFade == null) { Debug.LogError("Error: Cant find BlackFade on scene"); return; }
    }

    private IEnumerator ShowLogoRoutine()
    {
        yield return new WaitForSecondsRealtime(3f);
        _blackFade.FadeOut();
        yield return new WaitForSecondsRealtime(8f);
        _blackFade.FadeIn(() =>
        {
            SceneManager.LoadScene(_mainMenuSceneName);
        });       
    }
}

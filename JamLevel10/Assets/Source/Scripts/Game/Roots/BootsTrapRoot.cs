using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootsTrapRoot : CompositeRoot
{
    [SerializeField] private GameObject _logo;

    private BlackFade _blackFade;

    public override void Compose()
    {
        GetOtherLinks();
        StartCoroutine(ShowLogoRoutine());
    }

    private void GetOtherLinks()
    {
        _blackFade = FindAnyObjectByType<BlackFade>();
        if (_blackFade == null) { Debug.LogError("Error: Cant find BlackFade on scene"); return; }
    }

    private IEnumerator ShowLogoRoutine()
    {
        
        yield return new WaitForSecondsRealtime(2f);

        SceneManager.LoadScene();
    }
}

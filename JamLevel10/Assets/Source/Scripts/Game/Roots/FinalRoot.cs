using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalRoot : CompositeRoot
{
    [SerializeField] private BlackFade _fade;

    public override void Compose()
    {
        _fade.FadeOut();

        StartCoroutine(EndRoutine());
    }

    private IEnumerator EndRoutine()
    {
        yield return new WaitForSecondsRealtime(17f);
        _fade.FadeIn(() =>
        {
            SceneManager.LoadScene(1);
        });
    }
}

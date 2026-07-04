using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CanvasVideoPlayer : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    private bool _started;

    private void Start()
    {
        videoPlayer.playOnAwake = false;
        videoPlayer.waitForFirstFrame = true;

        videoPlayer.prepareCompleted += OnPrepared;
        videoPlayer.errorReceived += OnError;
        videoPlayer.loopPointReached += OnVideoFinished;

        videoPlayer.Prepare();
    }

    private void OnPrepared(VideoPlayer vp)
    {
        Debug.Log("VIDEO PREPARED");

        if (_started) return;

        _started = true;
        vp.Play();
    }

    private void OnError(VideoPlayer vp, string message)
    {
        Debug.LogError("VIDEO ERROR: " + message);
    }

    private void OnVideoFinished(VideoPlayer source)
    {
        SceneManager.LoadScene(1);
    }

    private void OnDestroy()
    {
        videoPlayer.prepareCompleted -= OnPrepared;
        videoPlayer.loopPointReached -= OnVideoFinished;
        videoPlayer.errorReceived -= OnError;
    }
}
using System;
using UnityEngine;
using UnityEngine.Video;

public class CanvasVideoPlayer : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    private FinalCutScene _cutScene;

    public void Initialize(FinalCutScene cutScene)
    {
        _cutScene = cutScene;
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void OnDestroy()
    {
        videoPlayer.loopPointReached -= OnVideoFinished;
    }

    public void Play()
    {
        videoPlayer.Play();
    }

    public void Stop()
    {
        videoPlayer.Stop();
    }

    public void Pause()
    {
        videoPlayer.Pause();
    }

    private void OnVideoFinished(VideoPlayer source)
    {
        _cutScene.OnVideoEnd();
    }
}

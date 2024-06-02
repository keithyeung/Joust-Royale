using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class VideoPlaying : MonoBehaviour
{
    public AudioSource audioSource;
    public VideoClip videoClip;
    public VideoPlayer videoPlayer;
    public float idleTime = 11.5f;

    private LobbyControls lb_control;

    private float idleTimer = 0f;
    public bool isPlayingVideo = false;

    private void Start()
    { 
        lb_control = ServiceLocator.instance.GetService<LobbyControls>();
        videoPlayer.clip = videoClip;
        
        StopVideo();
    }

    void Update()
    {
        if (lb_control.pressAToStart.activeSelf != true)
        {
            return;
        }
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleTime && !isPlayingVideo)
        {
            PlayVideo();
            audioSource.Stop();
        }
        
    }

    void PlayVideo()
    {
        videoPlayer.gameObject.transform.parent.gameObject.SetActive(true);
        videoPlayer.Play();
        isPlayingVideo = true;
        // Optionally, hide UI elements or show a different UI.
    }

    public void StopVideo()
    {
        //Debug.Log("StopVideo was called");
        if(isPlayingVideo)
        {
            audioSource.Play();
        }
        videoPlayer.Stop();
        videoPlayer.time = 0f;
        idleTimer = 0f; 
        
        isPlayingVideo = false;
        videoPlayer.gameObject.transform.parent.gameObject.SetActive(false);
        // Optionally, show UI elements again.
    }
}

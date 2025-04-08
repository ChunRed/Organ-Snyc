using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class temperture_count : MonoBehaviour
{
    
    public VideoClip increase_clip;
    public VideoClip decrease_clip;

    public bool trigger = false;
    bool trigger_pass = false;

    //video player
    private VideoPlayer _videoPlayer;

    public float state = 0f;

    void Start()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.isLooping = false;
        _videoPlayer.Play();
        state = 1f;

        _videoPlayer.loopPointReached += CheckOver;
    }

    
    void Update()
    {   

        //停止播放狀態
        if(state == 0f){
            if(trigger_pass != trigger){
                if( trigger ){
                _videoPlayer.clip = increase_clip;
                _videoPlayer.Play();
                _videoPlayer.loopPointReached += CheckOver;
                state = 1f;
                }
                else{
                    _videoPlayer.clip = decrease_clip;
                    _videoPlayer.Play();
                    _videoPlayer.loopPointReached += CheckOver;
                    state = 1f;
                }
                trigger_pass = trigger;
            }
        }
    }


    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        //Video Is Ove
        state = 0f;
    }


    


}

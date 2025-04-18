using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class video_manager : MonoBehaviour
{
    //video player
    private VideoPlayer _videoPlayer;

    public VideoClip lamp_clip;
    public VideoClip ambient_clip;

    private float pass_state = 0f;

    void Start()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        
    }

    
    void Update()
    {   
        if(MainPipeLine.instance.State == 1f && pass_state != MainPipeLine.instance.State){
            _videoPlayer.clip = lamp_clip;
            _videoPlayer.isLooping = false;
            _videoPlayer.Play();

            pass_state = MainPipeLine.instance.State;
        }
        else if(MainPipeLine.instance.State == 10f && pass_state != MainPipeLine.instance.State){
            _videoPlayer.clip = ambient_clip;
            _videoPlayer.isLooping = false;
            _videoPlayer.Play();

            pass_state = MainPipeLine.instance.State;
        }
        
        
    }
}

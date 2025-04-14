using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class temperture_count : MonoBehaviour
{
    public GameObject LightSensor;
    SunlightRaycastAudio _LightSensor;

    public VideoClip increase_clip;
    public VideoClip decrease_clip;

    public bool trigger = false;
    bool trigger_pass = false;

    //video player
    private VideoPlayer _videoPlayer;

    public float state = 0f;
    private float count = 0f;

    void Start()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.isLooping = false;
        _videoPlayer.Play();
        state = 1f;

        _LightSensor = LightSensor.GetComponent<SunlightRaycastAudio>();
    }

    
    void Update()
    {   
        trigger = _LightSensor.light_istrigger;

        //停止播放狀態
        if(state == 0f){
            if(trigger_pass != trigger){
                if( trigger ){
                _videoPlayer.clip = increase_clip;
                _videoPlayer.Play();
                state = 1f;
                }
                else{
                    _videoPlayer.clip = decrease_clip;
                    _videoPlayer.Play();
                    state = 1f;
                }
                trigger_pass = trigger;
            }
        }
        else if(state == 1f){
            count += Time.deltaTime;
            if(count > 7f){
                state = 0f;
                count = 0f;
            }
        }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class phone_display : MonoBehaviour
{
    public GameObject LightSensor;
    SunlightRaycastAudio _LightSensor;

    public Material M_screen;

    public bool trigger = false;

    //video player
    private VideoPlayer _videoPlayer;

    void Start()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _LightSensor = LightSensor.GetComponent<SunlightRaycastAudio>();
        _videoPlayer.isLooping = false;

    }

    void Update()
    {

        trigger = _LightSensor.light_istrigger;

        if(trigger == true){
            _videoPlayer.Play();
            M_screen.SetFloat("_pass", 1f);
        }
        else{
            _videoPlayer.Pause();
            M_screen.SetFloat("_pass", 0f);
        }

        if(MainPipeLine.instance.State == 9f){
            _videoPlayer.Pause();
        }
    }
    
}

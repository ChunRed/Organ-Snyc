using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource main_as; // 背景音樂檔案
    public AudioSource narration_as; //旁白聲音檔
    public AudioSource narration2_as; //旁白聲音檔
    
    public AudioSource ending_as; //結束音檔
    public AudioSource light_as; //direction light sound

    public AudioSource LightHit_as;
    
    public AudioMixer sceneMixer;  //場景總音量控制
    public AudioMixer lightMixer;  //光總音量控制
    public float playFromSec = 0f; // 從指定秒數播放背景聲音
    public float turnoffFromSec = 0f; //幾秒後背景音樂消失

    public bool turnoff_main = false;
    public bool turnoff_light = false;
    public float lightVol = 0f;
    public bool turnoff_scene = false;
    public float sceneVol = 0f;

    private bool narration_isPlay = false;
    private bool narration2_isPlay = false;
    private bool ending_isPlay = false;
    public bool LightHitPart = false;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

        main_as.spatialBlend = 0f; // 2D
        main_as.volume = 1f;
        main_as.playOnAwake = false;
        main_as.loop = true;
        main_as.Stop();

        narration_as.spatialBlend = 0f; // 2D
        narration_as.volume = 1f;
        narration_as.playOnAwake = false;
        narration_as.loop = false;
        narration_as.Stop();

        narration2_as.spatialBlend = 0f; // 2D
        narration2_as.volume = 1f;
        narration2_as.playOnAwake = false;
        narration2_as.loop = false;
        narration2_as.Stop();

        ending_as.spatialBlend = 0f; // 2D
        ending_as.volume = 1f;
        ending_as.playOnAwake = false;
        ending_as.loop = false;
        ending_as.Stop();

        light_as.volume = 0f;
        LightHit_as.volume = 0f;
    }


    void Update()
    {
        // if(setBgmVol != lastBgmVol){
        //     SetSoundVolume("bgmVol", setBgmVol);
        //     lastBgmVol = setBgmVol;
        // }
        if(turnoff_main){
            main_as.volume = Mathf.Lerp(main_as.volume, 0f, Time.deltaTime * 1f);
        }

        if(turnoff_light){
            lightVol = Mathf.Lerp(lightVol, -80f, Time.deltaTime * 0.2f);
            lightMixer.SetFloat("lightVol", lightVol);
        }else{
            lightVol = Mathf.Lerp(lightVol, -3f, Time.deltaTime * 0.2f);
            lightMixer.SetFloat("lightVol", lightVol);
        }

        if(turnoff_scene){
            sceneVol = Mathf.Lerp(sceneVol, -80f, Time.deltaTime * 0.2f);
            sceneMixer.SetFloat("sceneVol", sceneVol);
        }
    }
    public void play_main_as()
    {
        if (main_as != null)
        {
            // main_as.Stop();
            // main_as.time = playFromSec;
            main_as.Play();
        }
    }
    public void play_after_sec(){
        Invoke("play_main_as", playFromSec);
    }

    public void play_narration_as()
    {
        if (narration_as != null&& !narration_isPlay)
        {
            narration_as.Play();
            narration_isPlay = true;
        }
    }

    public void play_narration2_as()
    {
        if (narration2_as != null && !narration2_isPlay)
        {
            narration2_as.Play();
            narration2_isPlay = true;
        }
    }

    public void play_ending_as()
    {
        if (ending_as != null && !ending_isPlay)
        {
            ending_as.Play();
            ending_isPlay = true;
        }
    }

    public void turnoff_main_as()
    {
        if (main_as.isPlaying)
        {
            turnoff_main = true;
            main_as.loop = false;
        }
    }
    public void turnoff_after_sec()
    {
        Invoke("turnoff_main_as", turnoffFromSec);
    }

    public void turnoff_light_as()
    {
        if (light_as.isPlaying)
        {
            turnoff_light = true;
            light_as.loop = false;
        }
    }

    public void setSceneVolume()
    {
        turnoff_scene = true;
    }

    public void pause_narration_as()
    {
        if (narration_as != null && narration_as.isPlaying)
        {
            narration_as.Pause();
        }
    }
    // public void unPause_main_as()
    // {
    //     if (main_as != null)
    //     {
    //         main_as.UnPause();
    //     }
    // }
    // // public void SetSoundVolume(string soundGroup, float soundLevel)
    // // {
    //     masterMixer.SetFloat (soundGroup, soundLevel);
    // }
}

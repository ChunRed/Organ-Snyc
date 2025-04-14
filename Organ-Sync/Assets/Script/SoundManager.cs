using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource main_as; // 背景音樂檔案
    public AudioSource narration_as; //旁白聲音檔
    public AudioSource ending_as; //結束音檔
    
    // public AudioMixer masterMixer; 
    public float playFromSec = 0f; // 從指定秒數播放背景聲音
    // public float setBgmVol = 0f; // 背景音樂音量
    // private float lastBgmVol = 0f;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

        main_as.spatialBlend = 0f; // 2D
        main_as.playOnAwake = false;
        main_as.loop = true;
        main_as.Stop();

        narration_as.spatialBlend = 0f; // 2D
        narration_as.playOnAwake = false;
        narration_as.loop = false;
        narration_as.Stop();

        ending_as.spatialBlend = 0f; // 2D
        ending_as.playOnAwake = false;
        ending_as.loop = false;
        ending_as.Stop();
    }


    void Update()
    {
        // if(setBgmVol != lastBgmVol){
        //     SetSoundVolume("bgmVol", setBgmVol);
        //     lastBgmVol = setBgmVol;
        // }
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
        if (narration_as != null)
        {
            narration_as.Play();
        }
    }

    public void play_ending_as()
    {
        if (ending_as != null)
        {
            ending_as.Play();
        }
    }

    public void stop_main_as()
    {
        if (main_as != null)
        {
            main_as.Stop();
        }
    }

    // public void pause_main_as()
    // {
    //     if (main_as != null)
    //     {
    //         main_as.Pause();
    //     }
    // }
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

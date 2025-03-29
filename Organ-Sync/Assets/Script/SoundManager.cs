using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource main_audio; // 背景音樂檔案
    
    public AudioMixer masterMixer; 
    public float playFromSec = 0f; // 從指定秒數播放背景聲音
    public float setMasterVol = 0f; // 主音量控制
    public float setBgmVol = 0f; // 背景音樂音量
    public float setLightSoundVol = 0f; // 背景音樂音量
    private float lastMasterVol = 0f;
    private float lastBgmVol = 0f;
    private float lastLightSoundVol = 0f;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        main_audio = gameObject.GetComponent<AudioSource>();
        if (main_audio == null)
        {
            main_audio = gameObject.AddComponent<AudioSource>();
        }
        main_audio.spatialBlend = 1f; // 2D
        // main_audio.playOnAwake = false;
        // main_audio.loop = false;
        // main_audio.Stop();
    }


    void Update()
    {
        if(setMasterVol != lastMasterVol){
            SetSoundVolume("masterVol", setMasterVol);
            lastMasterVol = setMasterVol;
        }
        if(setBgmVol != lastBgmVol){
            SetSoundVolume("bgmVol", setBgmVol);
            lastBgmVol = setBgmVol;
        }
        if(setLightSoundVol != lastLightSoundVol){
            SetSoundVolume("lightSoundVol", setLightSoundVol);
            lastLightSoundVol = setLightSoundVol;
        }
    }
    public void play_main_audio()
    {
        if (main_audio != null)
        {
            main_audio.Stop();
            main_audio.time = playFromSec;
            main_audio.Play();
        }
    }

    public void stop_main_audio()
    {
        if (main_audio != null)
        {
            main_audio.Stop();
        }
    }

    public void pause_main_audio()
    {
        if (main_audio != null)
        {
            main_audio.Pause();
        }
    }
    public void unPause_main_audio()
    {
        if (main_audio != null)
        {
            main_audio.UnPause();
        }
    }
    public void SetSoundVolume(string soundGroup, float soundLevel)
    {
        masterMixer.SetFloat (soundGroup, soundLevel);
    }
}

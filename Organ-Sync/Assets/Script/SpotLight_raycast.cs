using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLight_raycast : MonoBehaviour
{
    
    
    public AudioSource audioSource;  // 音效來源
    public Light spotlight;   //spotlight
    public LayerMask obstacleLayer;  // 牆的圖層
    public float NoLightVol = 0.1f; //沒光時的音量
    public float LightVol = 1f; //照光時的音量

    [Header("判斷物件使否有被光線照到")]
    public bool light_istrigger = false;



    //燈光控制
    [Header("光線參數調整")]
    [Range(50, 15000)]
    public int intensity = 1000;
    public Color color = Color.white;
    public bool control_light = true;
    private bool light_change = false;

    [Header("觸發場景轉換用")] 
    public bool isLightHitPartTrigger = false;


    void Start()
    {
        
    }

    void Update()
    {
        
        
        //如果被spotlight照到則發出聲音
        Vector3 lightDirection_s = (transform.position - spotlight.transform.position).normalized;


        //判斷是否啟用效果
        if(MainPipeLine.instance.Lamp_Raycast_Effect){

            // 測試玩家是否在光錐內
            if (Vector3.Angle(spotlight.transform.forward, lightDirection_s) <= spotlight.spotAngle / 2)
            {
                //在光錐內則raycast
                if (Physics.Raycast(spotlight.transform.position, lightDirection_s, out RaycastHit hit, Mathf.Infinity, obstacleLayer))
                {
                    // 控制聲音
                    audioSource.volume = Mathf.Lerp(audioSource.volume, LightVol, Time.deltaTime * 3f);
                    Debug.Log("sound on");

                    if(SoundManager.instance.LightHitPart)//觸發場景轉換用
                    {
                        if(!isLightHitPartTrigger)
                        {
                            SoundManager.instance.LightHit_as.Play();
                            isLightHitPartTrigger = true;
                        }
                        SoundManager.instance.LightHit_as.volume = Mathf.Lerp(SoundManager.instance.LightHit_as.volume, LightVol, Time.deltaTime * 3f);
                    }

                    //控制燈光
                    if(control_light && light_change){
                        DAC_Light.instance.Lamp_intensity = intensity;
                        DAC_Light.instance.Lamp_color = color;
                        light_change = false;
                        light_istrigger = true;
                        ControllerKeepAlive.instance.SendKeepAlivePulse(0.8f, 2f);
                        
                    }


                }
                else
                {
                    // 控制聲音
                audioSource.volume = Mathf.Lerp(audioSource.volume, NoLightVol, Time.deltaTime * 3f);


                    //控制燈光
                    if(!light_change){
                        DAC_Light.instance.Lamp_intensity = 1000;
                        DAC_Light.instance.Lamp_color = Color.white;
                        light_change = true;
                        light_istrigger = false;
                        
                    }
                }
            }
            else
            {
                audioSource.volume = Mathf.Lerp(audioSource.volume, NoLightVol, Time.deltaTime * 3f);

                 if(SoundManager.instance.LightHitPart) //觸發場景轉換用
                {
                    SoundManager.instance.LightHit_as.volume = Mathf.Lerp(SoundManager.instance.LightHit_as.volume, NoLightVol, Time.deltaTime * 3f);
                    isLightHitPartTrigger = false;
                }


                //控制燈光
                if(!light_change){
                    DAC_Light.instance.Lamp_intensity = 1000;
                    DAC_Light.instance.Lamp_color = Color.white;
                    light_change = true;
                    light_istrigger = false;
                    
                }
            }
        }
    }
}

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


    //燈光控制
    [Range(50, 15000)]
    public int intensity = 1000;
    public Color color = Color.white;
    public bool control_light = true;
    private bool light_change = false;




    void Start()
    {
        
    }

    void Update()
    {
        
        
        // //如果被spotlight照到則發出聲音
        // Vector3 lightDirection_s = (transform.position - spotlight.transform.position).normalized;



        // //判斷是否啟用效果
        // if(MainPipeLine.instance.Lamp_Raycast_Effect){

        //     // 測試玩家是否在光錐內
        //     if (Vector3.Angle(spotlight.transform.forward, lightDirection_s) <= spotlight.spotAngle / 2)
        //     {
        //         //在光錐內則raycast
        //         if (Physics.Raycast(spotlight.transform.position, lightDirection_s, out RaycastHit hit, Mathf.Infinity, obstacleLayer))
        //         {
        //             // 控制聲音
        //             audioSource.volume = Mathf.Lerp(audioSource.volume, LightVol, Time.deltaTime * 3f);
        //             Debug.Log("sound on");

        //             //控制燈光
        //             if(control_light && light_change){
        //                 DAC_Light.instance.Lamp_intensity = intensity;
        //                 DAC_Light.instance.Lamp_color = color;
        //                 light_change = false;
                        
        //             }


        //         }
        //         else
        //         {
        //             // 控制聲音
        //         audioSource.volume = Mathf.Lerp(audioSource.volume, NoLightVol, Time.deltaTime * 3f);


        //             //控制燈光
        //             if(!light_change){
        //                 DAC_Light.instance.Lamp_intensity = 1000;
        //                 DAC_Light.instance.Lamp_color = Color.white;
        //                 light_change = true;
                        
        //             }
        //         }
        //     }
        //     else
        //     {
        //         audioSource.volume = Mathf.Lerp(audioSource.volume, NoLightVol, Time.deltaTime * 3f);


        //         //控制燈光
        //         if(!light_change){
        //             DAC_Light.instance.Lamp_intensity = 1000;
        //             DAC_Light.instance.Lamp_color = Color.white;
        //             light_change = true;
                    
        //         }
        //     }
        // }
    }
}

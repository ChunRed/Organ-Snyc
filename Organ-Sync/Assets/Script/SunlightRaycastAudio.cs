using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunlightRaycastAudio : MonoBehaviour
{
    public AudioSource audioSource;  // 音效來源
    public Light directionalLight;   // Directional Light
    // public Light[] spotlight;   //spotlight
    public LayerMask obstacleLayer;  // 牆的圖層
    public float NoLightVol = 0f; //沒光時的音量
    public float LightVol = 1f; //照光時的音量

    [Header("物品第一次被光照到時才開始播聲音")]
    public bool first_hit = false;

    [Header("判斷物件使否有被光線照到")]
    public bool light_istrigger = false;

    


    //燈光控制
    [Header("光線參數調整")]
    [Range(50, 50000)]
    public int intensity = 3000;

    
    public Color color1 = new Color (180, 180, 180, 180);
    public Color color2 = new Color (180, 180, 180, 180);
    public Color color3 = new Color (180, 180, 180, 180);
    public Color color4 = new Color (180, 180, 180, 180);

    public bool control_light = true;
    private bool light_change = true;


    void Setup(){
        audioSource.volume = 0f;
        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.Stop();
    }

    void Update()
    {
        //如果被Direction light照到則發出聲音
        if (directionalLight == null || audioSource == null) return;

        // 計算光照射方向（Directional Light 的 forward 方向是反的，所以要取 -forward）
        Vector3 lightDirection = -directionalLight.transform.forward;
        Vector3 rayOrigin = GetComponent<Collider>().bounds.center;



        
        //判斷是否啟用效果
        if(MainPipeLine.instance.Window_Raycast_Effect){

            // 從玩家位置向D light發射 Ray
            // if(Physics.Raycast(Org,Dir,(if hit do sth),distance,mask))
            if (Physics.Raycast(rayOrigin, lightDirection, Mathf.Infinity, obstacleLayer))
            {
                // Ray 碰到牆，表示玩家在陰影中
                // Debug.Log("player NOT in light");
                audioSource.volume = Mathf.Lerp(audioSource.volume, NoLightVol, Time.deltaTime * 3f);



                //MARK: 預設
                if(!light_change){

                    //改變光的亮度
                    DAC_Light.instance.intensity = 1000;

                    //改變光的強度
                    // DAC_Light.instance.color1 = new Color (255, 100, 100);
                    // DAC_Light.instance.color2 = new Color (110, 255, 110);
                    // DAC_Light.instance.color3 = new Color (100, 150, 255);
                    // DAC_Light.instance.color4 = new Color (255, 150, 100);

                    //改變Volume
                    Shader_ctrl.instance.Noise = 0.115f;
                    Shader_ctrl.instance.Contrast = -50f;


                    light_change = true;
                    light_istrigger = false;
                }
                
                
            }



            else
            {
                // 沒有碰到牆，表示玩家在光束內
                // Debug.Log("player in light");
                if(!first_hit){
                    audioSource.Play();
                    first_hit = true;
                }else{
                    audioSource.volume = Mathf.Lerp(audioSource.volume, LightVol, Time.deltaTime * 3f);
                }


                //MARK: light sensor trigger 後對光造成的數值變化
                if(control_light && light_change){

                    //改變光的亮度
                    DAC_Light.instance.intensity = intensity;

                    //改變光的強度
                    DAC_Light.instance.color1 = color1;
                    DAC_Light.instance.color2 = color2;
                    DAC_Light.instance.color3 = color3;
                    DAC_Light.instance.color4 = color4;

                    light_change = false;
                    light_istrigger = true;
                }
            }



        }
    }



    void ChangeColor(){
        //改變光的亮度
        DAC_Light.instance.intensity = intensity;

        //改變光的強度
        DAC_Light.instance.color1 = color1;
        DAC_Light.instance.color2 = color2;
        DAC_Light.instance.color3 = color3;
        DAC_Light.instance.color4 = color4;
    }
}

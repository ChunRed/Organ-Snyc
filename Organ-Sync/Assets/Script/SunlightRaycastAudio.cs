using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunlightRaycastAudio : MonoBehaviour
{
    public AudioSource audioSource;  // 音效來源
    public Light directionalLight;   // Directional Light
    public Light[] spotlight;   //spotlight
    public LayerMask obstacleLayer;  // 牆的圖層
    public float NoLightVol = 0.1f; //沒光時的音量
    public float LightVol = 1f; //照光時的音量

    void Update()
    {
        //如果被Direction light照到則發出聲音
        if (directionalLight == null || audioSource == null) return;

        // 計算光照射方向（Directional Light 的 forward 方向是反的，所以要取 -forward）
        Vector3 lightDirection = -directionalLight.transform.forward;
        Vector3 rayOrigin = GetComponent<Collider>().bounds.center;

        // 從玩家位置向D light發射 Ray
        // if(Physics.Raycast(Org,Dir,(if hit do sth),distance,mask))
        if (Physics.Raycast(rayOrigin, lightDirection, Mathf.Infinity, obstacleLayer))
        {
            // Ray 碰到牆，表示玩家在陰影中
            // Debug.Log("player NOT in Spotlight");
            audioSource.volume = Mathf.Lerp(audioSource.volume, NoLightVol, Time.deltaTime * 3f);
        }
        else
        {
            // 沒有碰到牆，表示玩家在光束內
            Debug.Log("player in Spotlight");
            audioSource.volume = Mathf.Lerp(audioSource.volume, LightVol, Time.deltaTime * 3f);
        }
        //------------------------------------------------------------------------------------------------------------
        //如果被spotlight照到則發出聲音
        for(int i = 0; i < spotlight.Length; i++){
            Vector3 lightDirection_s = (transform.position - spotlight[i].transform.position).normalized;

            // 測試玩家是否在光錐內
            if (Vector3.Angle(spotlight[i].transform.forward, lightDirection_s) <= spotlight[i].spotAngle / 2)
            {
                //在光錐內則raycast
                if (Physics.Raycast(spotlight[i].transform.position, lightDirection_s, out RaycastHit hit, Mathf.Infinity, obstacleLayer))
                {
                    Debug.Log("player in Spotlight");
                    audioSource.volume = Mathf.Lerp(audioSource.volume, LightVol, Time.deltaTime * 3f);
                }
                else
                {
                    audioSource.volume = Mathf.Lerp(audioSource.volume, NoLightVol, Time.deltaTime * 3f);
                }
            }
            else
            {
                audioSource.volume = Mathf.Lerp(audioSource.volume, NoLightVol, Time.deltaTime * 3f);
            }
        }
        
    }
    
}

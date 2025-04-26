using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class select_intro : MonoBehaviour
{
    public Material select_material;
    float select_emission = 0f;

    [Header("茶壺光感測")]
    public GameObject teatopSensor;
    SunlightHitKitchen _teatopSensor;
    float selectTime_count = 0f;
    public bool get_light = false;
    float select_pass = 0f;


    [Header("UI材質")]
    public Material M_select_intro; 
    

    void Start()
    {
        select_material.SetFloat("_emission", 0f);
        M_select_intro.SetFloat("_change", 0f);

        _teatopSensor = teatopSensor.GetComponent<SunlightHitKitchen>();
    }

    void Update()
    {   
        
        get_light = _teatopSensor.light_istrigger;
        if(get_light && MainPipeLine.instance.Select_Intro){
            selectTime_count += Time.deltaTime;
            if(selectTime_count > 18f && selectTime_count < 23f ) {
                select_pass = Mathf.Lerp(select_pass, 1f, 1f * Time.deltaTime);
                M_select_intro.SetFloat("_change", select_pass);
            }else if(selectTime_count >= 23f ){
                MainPipeLine.instance.Select_Intro = false;
            }else{
                M_select_intro.SetFloat("_change", 0f);
            }
        }
        else{
            selectTime_count = 0f;
        }
        

        //觸發select互動
        if(MainPipeLine.instance.Select_Intro) select_emission = Mathf.Lerp(select_emission, 1f, 0.5f * Time.deltaTime);
       
        else select_emission = Mathf.Lerp(select_emission, 0f, 0.5f * Time.deltaTime);
        
        //改變UI & 茶壺材質
        select_material.SetFloat("_emission", select_emission * 300f);
        M_select_intro.SetFloat("_pass", select_emission);
        
    }
}

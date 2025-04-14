using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class ambient_light : MonoBehaviour
{

    //環境光-1
    [Header("環境光 - 1 - 參數")]
    public GameObject ambient_light1;
    private bool light1_trigger = false;
    private  HDAdditionalLightData _ambient_light1;
    private bool is_trigger = false;
   




    void Start()
    {
        _ambient_light1 = ambient_light1.GetComponent<HDAdditionalLightData>();
    }


    void Update()
    {
        light1_trigger = MainPipeLine.instance.ambient_light;
        LightTrigger(_ambient_light1, light1_trigger);
    }



    void LightTrigger(HDAdditionalLightData light, bool trigger){
        if(trigger){
            if( !is_trigger){
                is_trigger = true;
            }
            
            light.intensity =  Mathf.Lerp(light.intensity, 40000, 0.2f * Time.deltaTime);
        }
        else{
            if(is_trigger){
                is_trigger = false;
            }
            
            light.intensity =  Mathf.Lerp(light.intensity, 0, 2f * Time.deltaTime);
        }
    }




    float Remap (float value, float from1, float to1, float from2, float to2) {
        if(value < from1) value = from1;
        if(value > to1) value = to1;
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}

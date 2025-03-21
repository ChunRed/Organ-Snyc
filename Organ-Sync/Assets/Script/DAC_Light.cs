using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DAC_Light : MonoBehaviour
{
    public GameObject VR_Hand;
    public GameObject Light;
    public GameObject Lamp;
    public Material window_emmision;

    private float window_light_pass = 1f;
    private float window_directionallight_pass = 1f;
    private  HDAdditionalLightData Directional_Light;
    private  HDAdditionalLightData Lamp_Light;

    [Range(0.1f, 1f)]
    public float smooth = 0.5f;
    public float point_light_offset = 90f; 



    //dynamic control Directional light 
    [Range(0, 15000)]
    public int intensity = 0;
    public Color color = new Color (180, 180, 180, 180);
    float window_intensity = 0f;


    //dynamic control Lamp light 
    [Range(0, 15000)]
    public int Lamp_intensity = 0;
    public Color Lamp_color = Color.black;
    public float Lamp_Smooth = 0.8f;



    private Vector3 targetAngle;
    private Vector3 currentAngle;
    private Vector3 currentAngle2;

    public  Vector3 Artnet_currentAngle;

    public static DAC_Light instance;

    public bool light_move = true;

 
    void Awake(){
        instance = this;    
    }


    void Start()
    {
        currentAngle = Light.transform.localEulerAngles;
        currentAngle2 = Lamp.transform.localEulerAngles;
        Artnet_currentAngle = Lamp.transform.localEulerAngles;

        Directional_Light = Light.GetComponent<HDAdditionalLightData>();
        Lamp_Light= Lamp.GetComponent<HDAdditionalLightData>();

        
    }

    
    void Update()
    {   
        


        //偵測搖桿轉動量
        targetAngle = VR_Hand.transform.localEulerAngles;



        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, targetAngle.x + 150f, smooth * Time.deltaTime),
            Mathf.LerpAngle(currentAngle.y, targetAngle.y - 30f, smooth * Time.deltaTime),
            Mathf.LerpAngle(currentAngle.z, targetAngle.z, smooth * Time.deltaTime));



        currentAngle2 = new Vector3(
            Mathf.LerpAngle(currentAngle2.x, targetAngle.x + point_light_offset, smooth*2.5f * Time.deltaTime), 
            Mathf.LerpAngle(currentAngle2.y, targetAngle.y, smooth*2.5f * Time.deltaTime), 0f);




        Artnet_currentAngle = new Vector3(
            Mathf.LerpAngle(Artnet_currentAngle.x, targetAngle.x, smooth * Time.deltaTime),
            Mathf.LerpAngle(Artnet_currentAngle.y, targetAngle.y, smooth * Time.deltaTime),
            Mathf.LerpAngle(Artnet_currentAngle.z, targetAngle.z, smooth * Time.deltaTime));




        //轉動方向光 & 檯燈
        if (light_move){
            Light.transform.localEulerAngles = currentAngle;
            Lamp.transform.localEulerAngles = currentAngle2;
        }
        

        
        window_intensity =  Mathf.Lerp(window_intensity, Remap(intensity, 0, 15000, 0, 1), 0.3f * Time.deltaTime);
        window_emmision.SetFloat("_emission", window_intensity);
        

        Directional_Light.intensity = (int)Mathf.Lerp(Directional_Light.intensity , intensity, 0.3f * Time.deltaTime);
        Directional_Light.color = Color.Lerp(Directional_Light.color , color, 0.3f * Time.deltaTime);

        Lamp_Light.intensity = (int)Mathf.Lerp(Lamp_Light.intensity , Lamp_intensity, Lamp_Smooth * Time.deltaTime);
        Lamp_Light.color = Color.Lerp(Lamp_Light.color , Lamp_color, Lamp_Smooth * Time.deltaTime);

    }








    float Remap (float value, float from1, float to1, float from2, float to2) {
        if(value < from1) value = from1;
        if(value > to1) value = to1;
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}

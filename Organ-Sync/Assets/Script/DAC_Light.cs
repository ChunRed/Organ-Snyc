using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DAC_Light : MonoBehaviour
{
    public GameObject VR_Hand;
    public GameObject Light;
    public GameObject test_cube;
    public Material window_emmision;

    private float window_light_pass = 1f;
    private float window_directionallight_pass = 1f;
    private  HDAdditionalLightData Directional_Light;

    [Range(0.1f, 1f)]
    public float smooth = 0.5f;



    //dynamic control light 
    [Range(50, 15000)]
    public int intensity = 3000;
    public Color color = new Color (180, 180, 180, 180);
    float window_intensity = 1f;


    private Vector3 targetAngle;
    private Vector3 currentAngle;

    public  Vector3 Artnet_currentAngle;

    public static DAC_Light instance;

    public bool light_move = true;
 
    void Awake(){
        instance = this;    
    }


    void Start()
    {
        currentAngle = Light.transform.localEulerAngles;
        Artnet_currentAngle = test_cube.transform.localEulerAngles;

        Directional_Light = Light.GetComponent<HDAdditionalLightData>();
    }

    
    void Update()
    {   

        targetAngle = VR_Hand.transform.localEulerAngles;

        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, targetAngle.x + 150f, smooth * Time.deltaTime),
            Mathf.LerpAngle(currentAngle.y, targetAngle.y - 30f, smooth * Time.deltaTime),
            Mathf.LerpAngle(currentAngle.z, targetAngle.z, smooth * Time.deltaTime));


        Artnet_currentAngle = new Vector3(
            Mathf.LerpAngle(Artnet_currentAngle.x, targetAngle.x, smooth * Time.deltaTime),
            Mathf.LerpAngle(Artnet_currentAngle.y, targetAngle.y, smooth * Time.deltaTime),
            Mathf.LerpAngle(Artnet_currentAngle.z, targetAngle.z, smooth * Time.deltaTime));

        if (light_move){
            Light.transform.localEulerAngles = currentAngle;
            test_cube.transform.localEulerAngles = currentAngle;
        }
        

        // ArtNet.instance.Light_Y = new_Y;
        // ArtNet.instance.Light_X = new_X;


        //if(Input.GetKeyDown("4")) window_light_flag = false;

        // if(MainPipeLine.instance.Directional_Light){
        //     if(window_light_pass > 0) window_light_pass -= 0.0007f;
        //     else {
        //         window_light_pass = 0f;
        //     }

        //     if(window_directionallight_pass > 0) window_directionallight_pass -= 0.00001f;
        //     else {
        //         window_directionallight_pass = 0f;
        //         MainPipeLine.instance.Directional_Light = false;
        //     }


        //     window_emmision.SetFloat("_emission", window_light_pass);
        //     Directional_Light.intensity *= window_directionallight_pass;
        // }
        window_intensity =  Mathf.Lerp(window_intensity, Remap(intensity, 50, 15000, 0, 1), 0.7f * Time.deltaTime);

        window_emmision.SetFloat("_emission", window_intensity);
        Directional_Light.intensity = (int)Mathf.Lerp(Directional_Light.intensity , intensity, 0.7f * Time.deltaTime);
        Directional_Light.color = Color.Lerp(Directional_Light.color , color, 0.7f * Time.deltaTime);
    }


    // void LightIntensity( int intensity, Color color){
    //     Directional_Light.intensity = (int)Mathf.Lerp(Directional_Light.intensity , intensity, 0.7f * Time.deltaTime);
    //     Directional_Light.color = Color.Lerp(Directional_Light.color , color, 0.7f * Time.deltaTime);
    // }


    float Remap (float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}

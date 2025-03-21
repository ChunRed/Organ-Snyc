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

    [Range(0.1f, 1f)]
    public float smooth = 0.5f;

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
        //Artnet_currentAngle = test_cube.transform.localEulerAngles;

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


        


        //轉動方向光 & 檯燈
        if (light_move){
            Light.transform.localEulerAngles = currentAngle;
        }
    }
}

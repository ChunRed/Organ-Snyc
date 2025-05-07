using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.XR;

public class DAC_Light : MonoBehaviour
{
    public GameObject VR_Hand;
    public GameObject Light;
    public GameObject Lamp;
    //public GameObject noedge_light;
    public GameObject Lamp_pedestal;
    public Material window_emmision;
    public Material Lamp_Light_emmision;
    public Material Lamp_head_emmision;

    private float window_light_pass = 1f;
    private float window_directionallight_pass = 1f;
    private  HDAdditionalLightData Directional_Light;
    private  HDAdditionalLightData Lamp_Light;

    [Range(0.1f, 1f)]
    public float smooth = 0.5f;
    public float point_light_offsetX = 90f; 
    public float point_light_offsetY = 0f; 



    //dynamic control Directional light 
    [Range(0, 15000)]
    public int intensity = 0;

    [Header("方向光顏色")]
    public Material directionalLight_material; // 方向光材質顏色
    public Color color1 = new Color (180, 180, 180, 180);
    public Color color2 = new Color (180, 180, 180, 180);
    public Color color3 = new Color (180, 180, 180, 180);
    public Color color4 = new Color (180, 180, 180, 180);
    private Color lerp_color1 = new Color (180, 180, 180, 180);
    private Color lerp_color2 = new Color (180, 180, 180, 180);
    private Color lerp_color3 = new Color (180, 180, 180, 180);
    private Color lerp_color4 = new Color (180, 180, 180, 180);
    float window_intensity = 0f;


    //dynamic control Lamp light 
    [Range(0, 15000)]
    public int Lamp_intensity = 0;
    public Color Lamp_color = Color.black;
    public float Lamp_Smooth = 0.8f;


    private Vector3 currentAngle;
    private Vector3 currentAngle2;

    public  Vector3 Artnet_currentAngle;

    public static DAC_Light instance;

    public bool light_move = true;
    public bool lamp_move = true;

    public GameObject Lamp_LightSensor;




    public XRNode controllerNode = XRNode.RightHand; // 可改成 LeftHand
    private Quaternion initialRotation;
    public bool isCalibrated = false;
    public Vector3 targetAngle = new Vector3(0f, 0f, 0f);
 
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
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);

        if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion currentRotation))
        {
            if (!isCalibrated)
            {
                // 第一次抓到數值就設定初始校正角度
                initialRotation = currentRotation;
                isCalibrated = true;
            }

            // 計算相對旋轉
            Quaternion relativeRotation = Quaternion.Inverse(initialRotation) * currentRotation;

            //mod 360
            // 轉換為 Euler 角，方便限制角度
            targetAngle.x = (relativeRotation.eulerAngles.x % 360 + 360) % 360;
            targetAngle.y = (relativeRotation.eulerAngles.y % 360 + 360) % 360;
            targetAngle.z = (relativeRotation.eulerAngles.z % 360 + 360) % 360;

            Artnet_currentAngle.x = (relativeRotation.eulerAngles.x + 180 % 360 + 360) % 360;
            Artnet_currentAngle.y = (relativeRotation.eulerAngles.y + 180 % 360 + 360) % 360;
            Artnet_currentAngle.z = targetAngle.z;
        }
        else{
            targetAngle = new Vector3(0f, 0f, 0f);
        }



        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, targetAngle.x + 150f, smooth * Time.deltaTime),
            Mathf.LerpAngle(currentAngle.y, targetAngle.y - 30f, smooth * Time.deltaTime),
            Mathf.LerpAngle(currentAngle.z, targetAngle.z, smooth * Time.deltaTime));



        currentAngle2 = new Vector3(
            Mathf.LerpAngle(currentAngle2.x, targetAngle.x + point_light_offsetX, smooth*2.5f * Time.deltaTime), 
            Mathf.LerpAngle(currentAngle2.y, (targetAngle.y + point_light_offsetY)*2f, smooth*2.5f * Time.deltaTime), 0f);




        //Artnet_currentAngle = new Vector3(
            // Mathf.LerpAngle(Artnet_currentAngle.x, (targetAngle.x % 360 + 360) % 360, 0.9f * Time.deltaTime),
            // Mathf.LerpAngle(Artnet_currentAngle.y, (targetAngle.y % 360 + 360) % 360, 0.9f * Time.deltaTime),
            // Mathf.LerpAngle(Artnet_currentAngle.z, (targetAngle.z % 360 + 360) % 360, 0.9f * Time.deltaTime));
            





        //轉動方向光 & 檯燈
        if (light_move){
            Light.transform.localEulerAngles = currentAngle;
            
        }
        

        
        window_intensity =  Mathf.Lerp(window_intensity, Remap(intensity, 0, 15000, 0, 1), 0.6f * Time.deltaTime);
        window_emmision.SetFloat("_emission", window_intensity);
        

        //MARK:改變方向光的強度
        Directional_Light.intensity = (int)Mathf.Lerp(Directional_Light.intensity , intensity, 0.3f * Time.deltaTime);


        //MARK:改變方向光的顏色
        lerp_color1 = Color.Lerp(lerp_color1, color1, 3f * Time.deltaTime);
        lerp_color2 = Color.Lerp(lerp_color2, color2, 3f * Time.deltaTime);
        lerp_color3 = Color.Lerp(lerp_color3, color3, 3f * Time.deltaTime);
        lerp_color4 = Color.Lerp(lerp_color4, color4, 3f * Time.deltaTime);



        if(Lamp_Light.intensity != 0){
            Lamp.transform.localEulerAngles = currentAngle2; 

            Lamp_Light.intensity = (int)Mathf.Lerp(Lamp_Light.intensity , Lamp_intensity, Lamp_Smooth * Time.deltaTime);
            Lamp_Light.color = Color.Lerp(Lamp_Light.color , Lamp_color, Lamp_Smooth * Time.deltaTime);

            float Lamp_emmision = Remap(Lamp_Light.intensity, 0, 15000, 0, 50);
            Lamp_head_emmision.SetColor("_Color", new Color(244*Lamp_emmision, 154*Lamp_emmision, 86*Lamp_emmision, 1));
            Lamp_head_emmision.SetFloat("_emission", Lamp_emmision*50f);
            Lamp_Light_emmision.SetFloat("_pass", Lamp_emmision*180f);


            directionalLight_material.SetColor("_Color1", lerp_color1);
            directionalLight_material.SetColor("_Color2", lerp_color2);
            directionalLight_material.SetColor("_Color3", lerp_color3);
            directionalLight_material.SetColor("_Color4", lerp_color4);
            
        }
        else{
            Destroy(Lamp);
            Destroy(Lamp_LightSensor);
            Destroy(Lamp_pedestal);
        }
    }








    float Remap (float value, float from1, float to1, float from2, float to2) {
        if(value < from1) value = from1;
        if(value > to1) value = to1;
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPipeLine : MonoBehaviour
{
    public static MainPipeLine instance;
 
    
    
    public GameObject VR_Camera;
    public GameObject VR_Passthrough;
    public GameObject volume;

    public GameObject p1_model;
    public GameObject p1_light;

    public float Move_Speed = 5f;
    public Vector3 position1 = new Vector3(0f, 0f, 0f);
    public Vector3 position2 = new Vector3(0f, 0f, 0f);
    public Vector3 position3 = new Vector3(0f, 0f, 0f);
    public Vector3 position4 = new Vector3(0f, 0f, 0f); 
    Vector3 new_position = new Vector3(0f, 0f, 0f);

    float speed = 0;

    [Range(0f, 10f)]
    public float State = 0f;

    public bool model_float = false;
    public bool Directional_Light = false;

    OVRPassthroughLayer passthrough;
    
    public TMPro.TextMeshProUGUI FPS; 
    private int lastFrameIndex;
    private float[] frameDeltaTimeArray;


    private void Awake(){
        instance = this;   
        frameDeltaTimeArray = new float[50];
    }
    private float CalculateFPS(){
        float total = 0f;
        foreach (float deltaTIme in frameDeltaTimeArray){
            total += deltaTIme;
        }
        return frameDeltaTimeArray.Length / total;
    }

    void Start()
    {
        passthrough = VR_Passthrough.GetComponent<OVRPassthroughLayer>();
        VR_Passthrough.SetActive(false);
        new_position = position1;
    }

    
    void Update()
    {
        movement();
        Set_Passthrough();

        frameDeltaTimeArray[lastFrameIndex] = Time.deltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;
        FPS.text = "FPS : " + Mathf.RoundToInt(CalculateFPS()).ToString();



        if(speed < Move_Speed*0.01f) speed += 0.0001f;
        else speed = Move_Speed*0.01f;


        
        if (Input.GetKey("z")) {
            new_position = position1;
            speed = 0;
        }
        if (Input.GetKey("x")) {
            new_position = position2;
            speed = 0;
        }
        if (Input.GetKey("c")) {
            new_position = position3;
            speed = 0;
        }

        VR_Camera.transform.position = Vector3.Lerp(VR_Camera.transform.position, new_position, speed * Time.deltaTime);


        if(State == 0f){
            if(VR_Camera.transform.position.z < -5f){
                Shader_ctrl.instance.trigger_flag = false;
                State = 1f;
            }
        }


        if(State == 1f){
            if(VR_Camera.transform.position.z < -6.3f){
                model_float = true;
                State = 2f;
            }
        }


        if(State == 2f){
            if(VR_Camera.transform.position.z < -8f){
                Directional_Light = true;
                State = 3f;
            }
        }

        

    }





















    private void Set_Passthrough(){
        
        if(VR_Passthrough.activeSelf == false) VR_Passthrough.SetActive(true);

        if (Input.GetKey("q"))
        {
            if(passthrough.textureOpacity < 0.8f) passthrough.textureOpacity += 0.01f;
            else {
                //volume.SetActive(false);
                passthrough.textureOpacity = 0.8f;
            }
        }  

        if(Input.GetKey("e")){
            //if(volume.activeSelf == false) volume.SetActive(true);
            if(passthrough.textureOpacity > 0f) passthrough.textureOpacity -= 0.01f;
            else passthrough.textureOpacity = 0f;
        }

        // passthroughLayer.overlayType = OVROverlay.OverlayType.Overlay;
        // passthroughLayer.textureOpacity = 0.3f;
        // environment.SetActive(false);
    }























    private void movement()
	{ 
		
        if (Input.GetKey("d"))
        {
            VR_Camera.transform.Translate(new Vector3(0.1f * Move_Speed, 0f, 0f) * Time.deltaTime );
        }  

            
        if (Input.GetKey("a"))
        {
            VR_Camera.transform.Translate(new Vector3(-0.1f * Move_Speed, 0f, 0f) * Time.deltaTime);
        }  
            
        if (Input.GetKey("w"))
        {
            VR_Camera.transform.Translate(new Vector3(0f, 0f, 0.1f * Move_Speed) * Time.deltaTime);
        }  
            
        if (Input.GetKey("s"))
        {
            VR_Camera.transform.Translate(new Vector3(0f, 0f, -0.1f * Move_Speed) * Time.deltaTime);
        }  
        if (Input.GetKey("up"))
        {
            VR_Camera.transform.Translate(new Vector3(0f, 0.1f * Move_Speed, 0f) * Time.deltaTime);
        } 
        if (Input.GetKey("down"))
        {
            VR_Camera.transform.Translate(new Vector3(0f, -0.1f * Move_Speed, 0f) * Time.deltaTime);
        } 
    }
}

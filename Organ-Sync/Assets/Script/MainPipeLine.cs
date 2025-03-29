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
    public bool Window_Raycast_Effect = false;
    public bool Light_Object = false;

    OVRPassthroughLayer passthrough;
    
    //UI
    public TMPro.TextMeshProUGUI FPS; 
    public GameObject intro;

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

        //FPS監控
        frameDeltaTimeArray[lastFrameIndex] = Time.deltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;
        FPS.text = "FPS : " + Mathf.RoundToInt(CalculateFPS()).ToString();


        //VR頭盔移動速度
        if(speed < Move_Speed*0.01f) speed += 0.0001f;
        else speed = Move_Speed*0.01f;


        //按鈕觸發判斷VR頭盔的移動位置
        if (Input.GetKey("2")) {
            new_position = position1;
            speed = 0;
        }
        if (Input.GetKey("3")) {
            new_position = position2;
            speed = 0;
        }
        if (Input.GetKey("4")) {
            new_position = position3;
            speed = 0;
        }






        //用VR頭盔的位置去判斷事件是否該觸發
        //VR_Camera.transform.position = Vector3.Lerp(VR_Camera.transform.position, new_position, speed * Time.deltaTime);





        // [ 檯燈環節 ] 教學模式，判斷是否達到目標後進入 [ 窗光環節 ]
        if(State == 0f){

            // 關閉方向光
            DAC_Light.instance.intensity = 0;
            DAC_Light.instance.color = Color.black;
            Directional_Light = false;

            // 關閉 window 的 raycast 偵測物件效果
            Window_Raycast_Effect = false;  


            // 關閉發亮物件
            Light_Object = false;
  

            //判斷是否達成目標      
            if (Input.GetKey("1")) {
              State = 1f; 
            }
        }






        // 進入[窗光環節] 關閉檯燈、開啟方向光、開啟窗戶 emmision
        else if(State == 1f){
            
            // 開啟方向光
            Invoke("Open_Window_Light", 18f);

            // 關閉台燈光
            DAC_Light.instance.Lamp_intensity = 0;
            DAC_Light.instance.Lamp_color = Color.black;
            DAC_Light.instance.Lamp_Smooth = 2f;

            //關閉 Intro UI Text
            Destroy(intro);
            
            


            // 開啟 raycast 偵測物件效果
            Window_Raycast_Effect = true;   
                    
            State = 2f;
        }






        //VR頭盔 : -5f  ->  material init
        else if(State == 2f){
            if(VR_Camera.transform.position.z < -5f){
                Shader_ctrl.instance.trigger_flag = false;
                State = 3f;
            }
        }






        //VR頭盔 : -6.3f  ->  物件飄起
        else if(State == 3f){
            if(VR_Camera.transform.position.z < -6.3f){
                model_float = true;
                State = 4f;
            }
        }






        //VR頭盔 : -8f  ->  方向光關閉
        else if(State == 4f){
            // if(VR_Camera.transform.position.z < -8f){
            //     Directional_Light = true;
            //     State = 3f;
            // }
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




    void Open_Window_Light(){
        DAC_Light.instance.intensity = 4000;
        DAC_Light.instance.color = Color.white;
        Directional_Light = true;


        // 開啟發亮物件
        Light_Object = true;
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

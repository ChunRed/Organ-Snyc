using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Rendering.HighDefinition;

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

    [Range(0f, 20f)]
    public float State = 0f;

    public bool model_float = false;
    public bool Directional_Light = false;
    public bool Window_Raycast_Effect = false;
    public bool Light_Object = false;

    OVRPassthroughLayer passthrough;
    
    //UI
    public TMPro.TextMeshProUGUI FPS; 
    public GameObject intro;
    public Material M_hand_icon_UD;
    public Material M_hand_icon_RL;
    public Material M_light_icon_UD;
    public Material M_light_icon_RL;
    public Material M_intro_model;
    public GameObject Intro_hand;
    public GameObject Intro_light;
    Animator A_hand;
    Animator A_light;
    float Intro_trigger_count_1 = 1f;
    float Intro_trigger_count_2 = 1f;
    float Intro_trigger_count_3 = 1f;
    private  HDAdditionalLightData Intro_Light;
    //UI


    //場景模型
    public GameObject Model;


    //檯燈亮起的延遲時間
    [Range(0f, 50f)]
    public float Lamp_trigger_delay = 35f;

    private int lastFrameIndex;
    private float[] frameDeltaTimeArray;

    private bool soundStart = false;


    //video player
    private VideoPlayer _videoPlayer;


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
        

        
        _videoPlayer = GetComponent<VideoPlayer>();

        A_hand = Intro_hand.GetComponent<Animator>();
        A_light = Intro_light.GetComponent<Animator>();

        M_hand_icon_RL.SetFloat("_pass", Intro_trigger_count_2);
        M_hand_icon_UD.SetFloat("_pass", Intro_trigger_count_1);
        M_light_icon_RL.SetFloat("_pass", Intro_trigger_count_2);
        M_light_icon_UD.SetFloat("_pass", Intro_trigger_count_1);
        M_intro_model.SetFloat("_pass", Intro_trigger_count_3);
        Intro_Light = intro.GetComponent<HDAdditionalLightData>();
        Intro_Light.intensity = 0;
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






        //用VR頭盔的位置去判斷事件是否該觸發
        if (new_position.z != 0f){
            VR_Camera.transform.position = Vector3.Lerp(VR_Camera.transform.position, new_position, speed * Time.deltaTime);
        }
        




        //MARK:STATE - 0
        //======================================================================
        //======================================================================
        // [ 檯燈環節 ] 教學模式，判斷是否達到目標後進入 [ 窗光環節 ]
        if(State == 0f){

            // 關閉方向光
            DAC_Light.instance.intensity = 0;
            DAC_Light.instance.color = Color.black;
            Directional_Light = false;

            // 關閉台燈光
            DAC_Light.instance.Lamp_intensity = 1;
            DAC_Light.instance.Lamp_color = Color.black;
            DAC_Light.instance.Lamp_Smooth = 2f;

            // 關閉 window 的 raycast 偵測物件效果
            Window_Raycast_Effect = false;  

            // 關閉發亮物件
            Light_Object = false;

            //判斷是否開始進入旁白
            if(Input.GetKey("n") && soundStart == false){
                soundStart = true;
                SoundManager.instance.play_after_sec();
                SoundManager.instance.play_narration_as();

                StartCoroutine(WaitChangeState(1f, Lamp_trigger_delay));
            }
        }




        //MARK:STATE - 1
        //======================================================================
        //======================================================================
        //開啟檯燈並等待音檔撥放完畢 ( 用檯燈照自己 ) 
        else if(State == 1f){
            
            // 開啟台燈光
            DAC_Light.instance.Lamp_intensity = 5000;
            DAC_Light.instance.Lamp_color = Color.white;
            DAC_Light.instance.Lamp_Smooth = 0.02f;

            //判斷音檔使否撥放完畢      
            if (Input.GetKey("1")) {
                if(soundStart == false){
                    SoundManager.instance.play_main_as();
                }
                State = 2f; 
            }
        }



//操作教學環節
//======================================================================
//======================================================================
//======================================================================
//======================================================================



        //MARK:STATE - 2
        //======================================================================
        //======================================================================
        //進入燈光操作說明環節 - 上下操作說明
        else if(State == 2f){
            
            //開啟操作UI說明
            TriggerIntro(false, 1);
            TriggerIntro(false, 3);

            //播放 " 上下操作 " 動畫
            A_hand.SetBool("up_down", true);
            A_light.SetBool("up_down", true);

            //判斷是否達成目標      
            if (Input.GetKey("2")) {
                State = 3f; 
            }
        }





        //MARK:STATE - 3
        //======================================================================
        //======================================================================
        //進入燈光操作說明環節 - 左右操作說明
        else if(State == 3f){

            TriggerIntro(true, 1);
            TriggerIntro(false, 2);

            
            //停止 " 上下操作 " 動畫
            A_hand.SetBool("up_down", false);
            A_light.SetBool("up_down", false);

            //播放 " 左右操作 " 動畫
            A_hand.SetBool("left_right", true);
            A_light.SetBool("left_right", true);

            //判斷是否達成目標      
            if (Input.GetKey("3")) {
                State = 4f; 
            }
        }





        //MARK:STATE - 4
        //======================================================================
        //======================================================================
        //關閉燈光操作說明環節
        //達成指定目標後進入 "窗光環節"
        else if(State == 4f){



            //停止 " 左右操作 " 動畫
            A_hand.SetBool("left_right", false);
            A_light.SetBool("left_right", false);

            //關閉操作UI說明
            TriggerIntro(true, 2);
            TriggerIntro(true, 3);


            //判斷是否達成目標      
            if (Input.GetKey("4")) {
                State = 5f; 
            }
        }


//======================================================================
//======================================================================
//======================================================================
//======================================================================




        //MARK:STATE - 5
        //======================================================================
        //======================================================================
        // 進入[窗光環節] 關閉檯燈
        else if(State == 5f){

            //刪除所有 Intro 物件
            Destroy(intro);
            
            // 關閉台燈光
            DAC_Light.instance.Lamp_intensity = 0;
            DAC_Light.instance.Lamp_color = Color.black;
            DAC_Light.instance.Lamp_Smooth = 2f;

           

            // 開啟 raycast 偵測物件效果
            Window_Raycast_Effect = true;   
                    
            StartCoroutine(WaitChangeState(6f, 15f));
        }



        //MARK:STATE - 6
        //======================================================================
        //======================================================================
        // 開啟方向光、開啟窗戶 emmision
        else if(State == 6f){
            DAC_Light.instance.intensity = 4000;
            DAC_Light.instance.color = Color.white;
            Directional_Light = true;

            // 開啟發亮物件
            Light_Object = true;

            State = 7f;
        }





        //MARK:STATE - 7
        //======================================================================
        //======================================================================
        //VR頭盔 : -5f  ->  material init
        else if(State == 7f){

            //移動VR頭盔的位置
            if (Input.GetKey("5")) {
                new_position = position2;
                speed = 0;
            }

            //若 position.z < -5f 則啟用 shader_ctrl.cs
            if(VR_Camera.transform.position.z < -5f){
                Shader_ctrl.instance.trigger_flag = false;
                State = 8f;
            }
        }





        //MARK:STATE - 8
        //======================================================================
        //======================================================================
        //VR頭盔 : -6.3f  ->  物件飄起
        else if(State == 8f){

            //若 position.z < -6.3f 則啟用 gravity_translate.cs
            if(VR_Camera.transform.position.z < -6.3f){
                model_float = true;


                //關閉窗光 離開[窗光環節]
                if(Input.GetKey("6")){
                    State = 9f;
                }
            }
        }





        //MARK:STATE - 9
        //======================================================================
        //======================================================================
        //VR頭盔 : -8f  ->  窗光關閉 離開[窗光環節]
        else if(State == 9f){
            
            // 關閉 raycast 偵測物件效果
            Window_Raycast_Effect = false; 

            // 關閉方向光、關閉窗戶 emmision
            DAC_Light.instance.intensity = 0;
            DAC_Light.instance.color = Color.black;
            
            // 關閉發亮物件
            Light_Object = false;

            StartCoroutine(WaitChangeState(10f, 30f));
        }



        //MARK:STATE - 10
        //======================================================================
        //======================================================================
        //進入 [無邊] 環節
        else if(State == 10f){
            
            //刪除模型
            Destroy(Model);

            //開啟無邊燈光

            //開啟無邊light sensor

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
    }




    


    IEnumerator WaitChangeState(float state, float time){ 

        State = 20f;
        yield return new WaitForSeconds(time);
        State = state;

        //play video in state 1
        if(state == 1f){
            _videoPlayer.isLooping = false;
            _videoPlayer.Play();
        }
    }





    void TriggerIntro(bool trigger, int state){
        if(trigger){
            if(state == 1){
                if(Intro_trigger_count_1 < 1f) Intro_trigger_count_1 += 0.01f;
                else Intro_trigger_count_1 = 1f;
            }
            else if(state == 2){
                if(Intro_trigger_count_2 < 1f) Intro_trigger_count_2 += 0.01f;
                else Intro_trigger_count_2 = 1f;
            }
            else if(state == 3){
                if(Intro_trigger_count_3 < 1f) Intro_trigger_count_3 += 0.01f;
                else Intro_trigger_count_3 = 1f;
            }
            
        }
        else{

            if(state == 1){
                if(Intro_trigger_count_1 > 0f) Intro_trigger_count_1 -= 0.01f;
                else Intro_trigger_count_1 = 0f;
            }
            else if(state == 2){
                if(Intro_trigger_count_2 > 0f) Intro_trigger_count_2 -= 0.01f;
                else Intro_trigger_count_2 = 0f;
            }
            else if(state == 3){
                if(Intro_trigger_count_3 > 0f) Intro_trigger_count_3 -= 0.01f;
                else Intro_trigger_count_3 = 0f;
            }
        }




        M_hand_icon_RL.SetFloat("_pass", Intro_trigger_count_2);
        M_light_icon_RL.SetFloat("_pass", Intro_trigger_count_2);
        M_hand_icon_UD.SetFloat("_pass", Intro_trigger_count_1);
        M_light_icon_UD.SetFloat("_pass", Intro_trigger_count_1);
        M_intro_model.SetFloat("_pass", Intro_trigger_count_3);

        //開啟 Intro 光源
        Intro_Light.intensity = 800 - (Intro_trigger_count_3*800);
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

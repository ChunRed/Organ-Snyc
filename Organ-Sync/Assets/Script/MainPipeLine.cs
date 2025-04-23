using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.XR;

public class MainPipeLine : MonoBehaviour
{
    public static MainPipeLine instance;
 
    
    //VR腳色參數
    [Header("VR腳色參數")]
    public GameObject VR_Camera;
    public GameObject VR_Passthrough;
    public GameObject volume;
    public bool VR_isMove = false;

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

    //物件飄起觸發
    [Header("物件飄起觸發")]
    public bool model_float = false;
    
    //燈光觸發
    [Header("燈光觸發")]
    public bool Directional_Light = false;
    public bool Window_Raycast_Effect = false;
    public bool Lamp_Raycast_Effect = false;
    public bool Light_Object = false;
    public bool  ambient_light = false;
    private float lamp_pass_count = 0f;


    OVRPassthroughLayer passthrough;
    
    //UI
    [Header("UI參數")]
    public TMPro.TextMeshProUGUI FPS; 
    public TMPro.TextMeshProUGUI STATE; 
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
    [Header("state4 目標說明UI文字")]
    public Material intro_text;
    //UI


    //場景模型
    [Header("場景模型")]
    public GameObject Model;
    public GameObject Smooth_Model;
    public bool DestroyModel = false;


    //等待延遲的延遲時間
    [Header("檯燈亮起的延遲時間")]
    [Range(0f, 50f)]
    public float Lamp_trigger_delay = 35f;
    [Header("教學開始的延遲時間")]
    [Range(0f, 120f)]
    public float Intro_trigger_delay = 5f;
    [Header("窗光環節體驗時間")]
    [Range(0f, 180f)]
    public float VRmove_trigger_delay = 120f;




    [Header("台燈光感測")]
    public GameObject lamp_light_sensor;
    SpotLight_raycast _lamp_light_sensor;

    public bool Select_Intro = false;
    public bool DMX_trigger = false;

    private int lastFrameIndex;
    private float[] frameDeltaTimeArray;
    private bool soundStart = false;
    private bool IEnumerator_flag = false;

    //手把震動觸發
    ControllerKeepAlive _ControllerKeepAlive;
    //Passthrough變數
    passthroughControl _passthroughControl;


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
        

        
        //_videoPlayer = GetComponent<VideoPlayer>();

        A_hand = Intro_hand.GetComponent<Animator>();
        A_light = Intro_light.GetComponent<Animator>();

        M_hand_icon_RL.SetFloat("_pass", Intro_trigger_count_2);
        M_hand_icon_UD.SetFloat("_pass", Intro_trigger_count_1);
        M_light_icon_RL.SetFloat("_pass", Intro_trigger_count_2);
        M_light_icon_UD.SetFloat("_pass", Intro_trigger_count_1);
        M_intro_model.SetFloat("_pass", Intro_trigger_count_3);
        Intro_Light = intro.GetComponent<HDAdditionalLightData>();
        Intro_Light.intensity = 0;

        _lamp_light_sensor = lamp_light_sensor.GetComponent<SpotLight_raycast>();
        _ControllerKeepAlive = GetComponent<ControllerKeepAlive>();
        _passthroughControl = GetComponent<passthroughControl>();
        

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


        STATE.text = "STATE : " + State;



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


            //Passthrough
            if(soundStart == false) _passthroughControl.LerpPassthrough(0.3f, 5f);
            else _passthroughControl.LerpPassthrough(0f, 0.5f);


            //判斷是否開始進入旁白
            if(Input.GetKey("n") && soundStart == false){
                soundStart = true;
                SoundManager.instance.play_after_sec();
                SoundManager.instance.play_narration_as();

                //觸發手把震動
                _ControllerKeepAlive.autoKeepAlive = true;

                _passthroughControl.LerpPassthrough(0f, 0.5f);

                //自動執行控制
                if (!IEnumerator_flag){
                    IEnumerator_flag = true;
                    StartCoroutine(WaitChangeState(0f, 1f, Lamp_trigger_delay)); // Lamp_trigger_delay => 35f
                    
                }
            }
        }




        //MARK:STATE - 1
        //======================================================================
        //======================================================================
        //開啟檯燈並等待音檔撥放完畢 ( 用檯燈照自己 ) 
        else if(State == 1f){

            //Passthrough
            _passthroughControl.LerpPassthrough(0f, 0.5f);
            
            // 開啟台燈光
            DAC_Light.instance.Lamp_intensity = 5000;
            DAC_Light.instance.Lamp_color = Color.white;
            DAC_Light.instance.Lamp_Smooth = 0.02f;
            Lamp_Raycast_Effect = true;

            //判斷音檔使否撥放完畢 
            if (Input.GetKey("1")) {
                if(soundStart == false){
                    SoundManager.instance.play_main_as();
                }
                State = 2f; 
            }


            //自動執行控制
            if (!IEnumerator_flag){
                IEnumerator_flag = true;
                StartCoroutine(WaitChangeState(1f, 2f, Intro_trigger_delay));
                
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


            //手動執行控制      
            if (Input.GetKey("2")) {
                State = 3f; 
            }

            //自動執行控制
            if (!IEnumerator_flag){
                IEnumerator_flag = true;
                StartCoroutine(WaitChangeState(2f, 3f, 16f));
                
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

            //手動執行控制       
            if (Input.GetKey("3")) {
                State = 4f; 
            }

            //自動執行控制
            if (!IEnumerator_flag){
                IEnumerator_flag = true;
                StartCoroutine(WaitChangeState(3f, 4f, 16f));
            }
        }





        //MARK:STATE - 4
        //======================================================================
        //======================================================================
        //關閉燈光操作說明環節
        //達成指定目標後進入 "窗光環節"
        else if(State == 4f){
            //state4 顯示目標說明UI文字
            //intro_text.SetFloat("_pass", 1f);
            

            //停止 " 左右操作 " 動畫
            A_hand.SetBool("left_right", false);
            A_light.SetBool("left_right", false);

            //關閉操作UI說明
            TriggerIntro(true, 2);
            TriggerIntro(true, 3);

            //開啟計時音效
            SoundManager.instance.LightHitPart = true;

            //判斷是否達成目標
            //台燈照自己超過5秒 
            if (_lamp_light_sensor.light_istrigger) {

                //pp glitch jitter 效果設為0
                Shader_ctrl.instance.Jitter = 0.6f;

                if(lamp_pass_count > 5f) {
                    State = 5f; 
                    //state4 關閉目標說明UI文字
                    //intro_text.SetFloat("_pass", 0f);
                }
                else lamp_pass_count += Time.deltaTime;
            }
            else {
                lamp_pass_count = 0f;
                //pp glitch jitter 效果設為0
                Shader_ctrl.instance.Jitter = 0f;
            }

            if (Input.GetKey("4")) {
                State = 5f; 
                //state4 關閉目標說明UI文字
                //intro_text.SetFloat("_pass", 0f);
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

            //pp glitch jump 效果設為0.2
            Shader_ctrl.instance.Jitter = 0.4f;
            
            //關閉計時音效
            SoundManager.instance.LightHitPart = false;
            SoundManager.instance.turnoff_light = true;

            //刪除所有 Intro 物件
            Destroy(intro);
            
            // 關閉台燈光
            DAC_Light.instance.Lamp_intensity = 0;
            DAC_Light.instance.Lamp_color = Color.black;
            DAC_Light.instance.Lamp_Smooth = 2f;
                    
            
            //自動執行控制
            if (!IEnumerator_flag){
                StartCoroutine(WaitChangeState(5f, 6f, 15f));
                IEnumerator_flag = true;
            }
        }



        //MARK:STATE - 6
        //======================================================================
        //======================================================================
        // 開啟方向光、開啟窗戶 emmision
        else if(State == 6f){

            //pp glitch jitter 效果設為0
            Shader_ctrl.instance.Jitter = 0f;
            //pp glitch jump 效果設為0.2
            Shader_ctrl.instance.Jitter = 0f;

            DAC_Light.instance.intensity = 4000;
            DAC_Light.instance.color = Color.white;
            Directional_Light = true;
            // 若旁白1未結束，停止旁白1
            SoundManager.instance.pause_narration_as();

            //旁白2開始
            SoundManager.instance.play_narration2_as();
            SoundManager.instance.turnoff_light = false;

            // 開啟發亮物件
            Light_Object = true;

            // 開啟 raycast 偵測物件效果
            Window_Raycast_Effect = true; 

            //自動執行控制
            if (!IEnumerator_flag){
                StartCoroutine(WaitChangeState(6f, 6.2f, 15f));
                IEnumerator_flag = true;
            }
        }


        //MARK:STATE - 6.2 / 6.3
        //======================================================================
        //======================================================================
        //Select互動引導環節
        else if(State == 6.2f){
            
            Select_Intro = true;
            SoundManager.instance.KitchenHitPart = true;
            State = 6.3f;
        }
        else if(State == 6.3f){
            if(!Select_Intro) State = 6.5f;
        }


        //MARK:STATE - 6.5
        //======================================================================
        //======================================================================
        //窗光環節體驗時間
        else if(State == 6.5f){
            SoundManager.instance.KitchenHitPart = false;
            //自動執行控制
            if (!IEnumerator_flag){
                StartCoroutine(WaitChangeState(6.5f, 7f, VRmove_trigger_delay));
                IEnumerator_flag = true;
            }
        }




        //MARK:STATE - 7
        //======================================================================
        //======================================================================
        //VR頭盔頭盔開始移動
        //VR頭盔 : -4.4f  ->  材質群組一 啟用
        else if(State == 7f){

            //觸發VR頭盔頭盔移動
            VR_isMove = true;
                
            //若 position.z < -5f 則啟用 shader_ctrl.cs
            if(VR_Camera.transform.position.z < -4.4f){
                Shader_ctrl.instance.trigger_flag1 = false;
                State = 7.5f;
            }
        }



        //MARK:STATE - 7.5
        //======================================================================
        //======================================================================
        //VR頭盔 : -5f  ->  材質群組二 啟用
        //觸發飄起物件時的BGM
        else if(State == 7.5f){
            //若 position.z < -5.5f 則啟用 shader_ctrl.cs
            if(VR_Camera.transform.position.z < -5f){
                //材質群組二 啟用
                Shader_ctrl.instance.trigger_flag2 = false;
                Shader_ctrl.instance.trigger_SM_Model = false;

                //觸發飄起物件時的BGM
                SoundManager.instance.play_ending_as();
                SoundManager.instance.turnoff_after_sec();
                

                //關閉窗光 離開[窗光環節]
                //自動執行控制
                if (!IEnumerator_flag){
                    StartCoroutine(WaitChangeState(8f, 9f, 150f));
                    IEnumerator_flag = true;
                }
            }
        }




        //MARK:STATE - 8
        //======================================================================
        //======================================================================
        //VR頭盔 : -6.3f  ->  物件飄起
        else if(State == 8f){

            //若 position.z < -7.5f 則啟用 Glitch效果
            if(VR_Camera.transform.position.z < -7.5f){

                //pp glitch 調整 lerp 速度
                Shader_ctrl.instance.PPLerp_Speed = 0.07f;
                
                Shader_ctrl.instance.Jitter = 0.3f;
                Shader_ctrl.instance.Block = 0.1f;
                Shader_ctrl.instance.Shake = 0.015f;

            }
            //若 position.z < -5.8f 則啟用 gravity_translate.cs
            else if(VR_Camera.transform.position.z < -5.8f){

                model_float = true;
                Destroy(Smooth_Model);

            }
        }





        //MARK:STATE - 9
        //======================================================================
        //======================================================================
        //VR頭盔 : -8f  ->  窗光關閉 離開[窗光環節]
        else if(State == 9f){

            //關閉聲音
            SoundManager.instance.turnoff_light_as();
            SoundManager.instance.setSceneVolume();
            
            // 關閉 raycast 偵測物件效果，將光的聲音調小
            Window_Raycast_Effect = false; 
            
            

            // 關閉方向光、關閉窗戶 emmision
            DAC_Light.instance.intensity = 0;
            DAC_Light.instance.color = Color.black;
            
            // 關閉發亮物件
            Light_Object = false;
            DestroyModel = true;
            

            StartCoroutine(WaitChangeState(9f, 10f, 30f));
        }



        //MARK:STATE - 10
        //======================================================================
        //======================================================================
        //進入 [無邊] 環節
        else if(State == 10f){

            //pp glitch 調整 
            Shader_ctrl.instance.PPLerp_Speed = 5f;
                
            Shader_ctrl.instance.Jitter = 0f;
            Shader_ctrl.instance.Block = 0f;
            Shader_ctrl.instance.Shake = 0f;
            
            //刪除模型
            Destroy(Model);

            //開啟無邊燈光
            ambient_light = true;

            //播放無邊聲音檔
            SoundManager.instance.play_ambientPass_as();

            //自動執行控制
            if (!IEnumerator_flag){
                StartCoroutine(WaitChangeState(10f, 11f, 85f));
                IEnumerator_flag = true;
            }

            

        }


        //MARK:STATE - 11
        //======================================================================
        //======================================================================
        //進入 [Passthrough] 環節
        else if(State == 11f){
            //Passthrough
            _passthroughControl.LerpPassthrough(0.6f, 0.1f);
            DMX_trigger = true;
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




    


    IEnumerator WaitChangeState(float from_state, float to_state, float time){ 

        State = from_state;
        float startTime = Time.time;
        while (Time.time - startTime < time)
        {
            yield return null; // 每一幀都檢查一次時間
        }

        State = to_state;
        IEnumerator_flag = false;
    }





    void TriggerIntro(bool trigger, int state){
        if(trigger){
            if(state == 1){
                if(Intro_trigger_count_1 < 1f) Intro_trigger_count_1 += 0.02f;
                else Intro_trigger_count_1 = 1f;
            }
            else if(state == 2){
                if(Intro_trigger_count_2 < 1f) Intro_trigger_count_2 += 0.02f;
                else Intro_trigger_count_2 = 1f;
            }
            else if(state == 3){
                if(Intro_trigger_count_3 < 1f) Intro_trigger_count_3 += 0.02f;
                else Intro_trigger_count_3 = 1f;
            }
            
        }
        else{

            if(state == 1){
                if(Intro_trigger_count_1 > 0f) Intro_trigger_count_1 -= 0.02f;
                else Intro_trigger_count_1 = 0f;
            }
            else if(state == 2){
                if(Intro_trigger_count_2 > 0f) Intro_trigger_count_2 -= 0.02f;
                else Intro_trigger_count_2 = 0f;
            }
            else if(state == 3){
                if(Intro_trigger_count_3 > 0f) Intro_trigger_count_3 -= 0.02f;
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

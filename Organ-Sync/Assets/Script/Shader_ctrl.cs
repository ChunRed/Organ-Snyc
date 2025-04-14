using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shader_ctrl : MonoBehaviour
{
    
    public static Shader_ctrl instance;
    void Awake(){
        instance = this;    
    }

    


    [Header("材質群組 1")]
    public  List <Material> M_List1 = new List <Material> ();
    public float transform_speed1 = 1f;
    public bool trigger_flag1 = true;
    private float Material_pass1 = 1f;


    [Header("材質群組 2")]
    public  List <Material> M_List2 = new List <Material> ();
    public float transform_speed2 = 1f;
    public bool trigger_flag2 = true;
    private float Material_pass2 = 1f;




    //糕模材質調整
    [Header("高模材質")]
    public Material SM_Model;
    private float opacity = 1f;
    private float normal = 0.46f;
    private float dark = 1f;
    public bool trigger_SM_Model = true;
    



    void Start()
    {
        
    }

   void Update()
    {


        //MARK:觸發 高模材質 
        //======================================================================
        //======================================================================
        if(trigger_SM_Model){

            // Rock Model
            if(opacity < 1f) opacity += 0.0003f * transform_speed1;
            else opacity = 1f;
            if(dark < 1f) dark += 0.001f * transform_speed1;
            else dark = 1f;
            if(normal < 0.46f) normal += 0.001f * transform_speed1;
            else normal = 0.46f;
        }
        else{
            // Rock Model
            if(opacity > 0f) opacity -= 0.005f * transform_speed1;
            else opacity = 0f;
            if(dark > 0f) dark -= 0.01f * transform_speed1;
            else dark = 0f;
            if(normal > 0f) normal -= 0.01f * transform_speed1;
            else normal = 0f;
        }








        //MARK:觸發 材質群組 1
        //======================================================================
        //======================================================================
        if(trigger_flag1){
            //Material_pass1l
            if(Material_pass1 < 1f) Material_pass1 += 0.0003f * transform_speed1;
            else Material_pass1 = 1f;
            
        }
        else{
            //Material_pass1
            if(Material_pass1 > 0f) Material_pass1 -= 0.005f * transform_speed1;
            else Material_pass1 = 0f;
        }









        //MARK:觸發 材質群組 2
        //======================================================================
        //======================================================================
        if(trigger_flag2){
            //Material_pass12
            if(Material_pass2 < 1f) Material_pass2 += 0.0003f * transform_speed2;
            else Material_pass2 = 1f;
            
        }
        else{
            //Material_pass2
            if(Material_pass2 > 0f) Material_pass2 -= 0.005f * transform_speed2;
            else Material_pass2 = 0f;
        }













        SM_Model.SetFloat("_opacity", opacity);
        SM_Model.SetFloat("_normal", normal);
        SM_Model.SetFloat("_dark", dark);
        
        for(int i=0; i<M_List1.Count; i++){
            M_List1[i].SetFloat("_pass", Material_pass1);
        }

        for(int i=0; i<M_List2.Count; i++){
            M_List2[i].SetFloat("_pass", Material_pass2);
        }
    }
}

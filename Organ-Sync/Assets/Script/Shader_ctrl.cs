using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shader_ctrl : MonoBehaviour
{
    public Material SM_Model;
    

    public Material M_Floor;
    public Material M_Wall;
    public Material M_Brick1;
    public Material M_Brick2;
    public Material M_rockgroup;
    public Material M_wood;
    public Material M_IronSheet;

    public static Shader_ctrl instance;
 
    void Awake(){
        instance = this;    
    }

    

    public float transform_speed = 1f;


    public bool trigger_flag = true;
    private float opacity = 1f;
    private float normal = 0.46f;
    private float dark = 1f;

    private float floor_pass = 1f;
    private float wall_pass = 1f;



    void Start()
    {
        
    }

   void Update()
    {
        // if(Input.GetKeyDown("1"))  trigger_flag = true;
        // if(Input.GetKeyDown("2"))  trigger_flag = false;

        if(trigger_flag){

            // Rock Model
            if(opacity < 1f) opacity += 0.0003f * transform_speed;
            else opacity = 1f;

            if(dark < 1f) dark += 0.001f * transform_speed;
            else dark = 1f;

            if(normal < 0.46f) normal += 0.001f * transform_speed;
            else normal = 0.46f;





            //Floor
            if(floor_pass < 1f) floor_pass += 0.0003f * transform_speed;
            else floor_pass = 1f;

            //Wall
            if(wall_pass < 1f) wall_pass += 0.0003f * transform_speed;
            else wall_pass = 1f;
            
        }
        else{

            // Rock Model
            if(opacity > 0f) opacity -= 0.005f * transform_speed;
            else opacity = 0f;

            if(dark > 0f) dark -= 0.01f * transform_speed;
            else dark = 0f;

            if(normal > 0f) normal -= 0.01f * transform_speed;
            else normal = 0f;

           


            //Floor
            if(floor_pass > 0f) floor_pass -= 0.005f * transform_speed;
            else floor_pass = 0f;

            //Wall
            if(wall_pass > 0f) wall_pass -= 0.005f * transform_speed;
            else wall_pass = 0f;
        }

        SM_Model.SetFloat("_opacity", opacity);
        SM_Model.SetFloat("_normal", normal);
        SM_Model.SetFloat("_dark", dark);
        
        M_Floor.SetFloat("_pass", floor_pass);
        M_Wall.SetFloat("_pass", wall_pass);
        M_Brick1.SetFloat("_pass", wall_pass);
        M_Brick2.SetFloat("_pass", wall_pass);
        M_IronSheet.SetFloat("_pass", wall_pass);
        M_rockgroup.SetFloat("_opacity", wall_pass);
        M_wood.SetFloat("_pass", wall_pass);
    }
}

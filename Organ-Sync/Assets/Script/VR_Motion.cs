using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_Motion : MonoBehaviour
{
    
    public GameObject VR_Camera;
    public GameObject VR_Passthrough;
    public GameObject volume;

    public GameObject p1_model;
    public GameObject p1_light;

    public float Move_Speed = 5f;

    OVRPassthroughLayer passthrough;
    

    void Start()
    {
        passthrough = VR_Passthrough.GetComponent<OVRPassthroughLayer>();
        VR_Passthrough.SetActive(false);
    }

    
    void Update()
    {
        movement();
        Set_Passthrough();



        if(VR_Camera.transform.position.z < -14f){
            p1_model.SetActive(false);
            p1_light.SetActive(false);
        }else {
            p1_model.SetActive(true);
            p1_light.SetActive(true);
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

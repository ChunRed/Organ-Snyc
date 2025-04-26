using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingObject : MonoBehaviour
{
    
    public  List <Material> M_List = new List <Material> ();

    public GameObject reflection_probe;


    float pass = 0f;
    public float smooth = 0.5f;
    public bool lightObjPass_flag = false;


    void Start()
    {
        
    }

    
    
    void Update()
    {   
        if(MainPipeLine.instance.DestroyModel == false){

            if( MainPipeLine.instance.Light_Object){
                reflection_probe.SetActive(true);
                pass = Mathf.Lerp(pass, 1f, smooth * Time.deltaTime);
                Debug.Log("test");
            }
            else{
                reflection_probe.SetActive(false);
                pass = Mathf.Lerp(pass, 0f, smooth * Time.deltaTime);
                
                if(pass<0.01f && MainPipeLine.instance.State == 9){
                    // lightObjPass_flag = true;
                    MainPipeLine.instance.DestroyModel = true;
                }

            }
        }


        for(int i=0; i<M_List.Count; i++){
            M_List[i].SetFloat("_pass", pass);
        }
    }
}

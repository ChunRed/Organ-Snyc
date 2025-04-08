using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingObject : MonoBehaviour
{
    
    public  List <Material> M_List = new List <Material> ();

    public GameObject reflection_probe;


    void Start()
    {
        
    }

    
    
    void Update()
    {
        if(MainPipeLine.instance.State != 10f){
            if(MainPipeLine.instance.Light_Object){
                reflection_probe.SetActive(true);
                for(int i=0; i<M_List.Count; i++){
                    M_List[i].SetFloat("_pass", 1f);
                }
            }
            else{
                reflection_probe.SetActive(false);
                for(int i=0; i<M_List.Count; i++){
                    M_List[i].SetFloat("_pass", 0f);
                }
            }
        }
    }
}

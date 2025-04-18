using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class passthroughControl : MonoBehaviour
{
    public OVRPassthroughLayer passthroughLayer;
    

    
    void Start()
    {
        passthroughLayer.textureOpacity = 0f;
    }


    public void LerpPassthrough(float value, float speed){
        passthroughLayer.textureOpacity = Mathf.Lerp(passthroughLayer.textureOpacity, value, Time.deltaTime * speed);
    }
    
    void Update()
    {

    }
}

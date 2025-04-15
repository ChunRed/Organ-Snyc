using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class passthroughControl : MonoBehaviour
{
    public static passthroughControl instance;
    public OVRPassthroughLayer passthroughLayer;
    public bool passthough_isOpen = false;
    public float fade_speed = 0.1f;
    
    private void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        passthroughLayer.textureOpacity = 0f;
    }

    
    void Update()
    {
        if (Input.GetKey("p"))
        {
            passthough_isOpen = true;
        }
        if (Input.GetKey("c"))
        {
            passthough_isOpen = false;
        }

        if(passthough_isOpen){
            passthroughLayer.textureOpacity = Mathf.Lerp(passthroughLayer.textureOpacity, 1f, Time.deltaTime * fade_speed);
        }else{
            passthroughLayer.textureOpacity = Mathf.Lerp(passthroughLayer.textureOpacity, 0f, Time.deltaTime * fade_speed);   
        }
    }
}

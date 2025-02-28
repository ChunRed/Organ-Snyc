using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_grvity : MonoBehaviour
{
    public Vector3 StartPosition;
    [Range(0f, 20f)]
    public float delay = 0f;

    bool flag = false;

    Rigidbody rigidbody;
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        
    }

    void Update()
    {
        if (Input.GetKey("5") && !flag)
        {
            Invoke("LightTrigger", delay);
            flag = true;
        }
    }


    void LightTrigger(){
        rigidbody.AddForce(StartPosition, ForceMode.Impulse);
        rigidbody.useGravity = true;
        CancelInvoke();
    }
}

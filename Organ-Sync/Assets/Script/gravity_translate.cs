using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravity_translate : MonoBehaviour
{
    [Range(0f, 10f)]
    public float delay_time = 0f;

    [Range(0.001f, 0.05f)]
    public float smooth = 0.01f;

    public Vector3 position = new Vector3(0f, 0f, 0f);
    public Vector3 rotation = new Vector3(0f, 0f, 0f);


    bool flag = true;
    float speed = 0;


    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey("3") && flag)
        {
            InvokeRepeating("Movement", delay_time, 0.4f * Time.deltaTime);
            flag = false;
        }  
    }



    void Movement(){

        

        if(speed < smooth) speed += 0.000005f;
        else speed = smooth;


        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);

        transform.localEulerAngles = new Vector3(
            Mathf.LerpAngle(transform.localEulerAngles.x, rotation.x, speed * Time.deltaTime),
            Mathf.LerpAngle(transform.localEulerAngles.y, rotation.y, speed * Time.deltaTime),
            Mathf.LerpAngle(transform.localEulerAngles.z, rotation.z, speed * Time.deltaTime));
    }


}

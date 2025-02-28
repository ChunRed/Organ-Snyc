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

    


    float speed = 0;
    float map = 1f;
    bool flag = true;


    void Start()
    {
        
    }

    void Update()
    {
        if (MainPipeLine.instance.model_float && flag) {
            InvokeRepeating("Movement", delay_time, 0.4f * Time.deltaTime);
            Debug.Log("test");
            flag = false;
        }
    }



    void Movement(){

        

        if(speed < smooth) speed += 0.000005f * map;
        else speed = smooth;


        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime * map);

        transform.localEulerAngles = new Vector3(
            Mathf.LerpAngle(transform.localEulerAngles.x, rotation.x, speed * Time.deltaTime * map),
            Mathf.LerpAngle(transform.localEulerAngles.y, rotation.y, speed * Time.deltaTime * map),
            Mathf.LerpAngle(transform.localEulerAngles.z, rotation.z, speed * Time.deltaTime * map));
    }


}

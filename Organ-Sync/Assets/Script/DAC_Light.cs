using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAC_Light : MonoBehaviour
{
    public GameObject VR_Hand;
    public GameObject Light;
    public GameObject test_cube;

    [Range(0.1f, 1f)]
    public float smooth = 0.5f;

    private Vector3 targetAngle;
    private Vector3 currentAngle;

    public  Vector3 Artnet_currentAngle;

    public static DAC_Light instance;
 
    void Awake(){
        instance = this;    
    }


    void Start()
    {
        currentAngle = Light.transform.localEulerAngles;
        Artnet_currentAngle = test_cube.transform.localEulerAngles;
    }

    
    void Update()
    {   

        targetAngle = VR_Hand.transform.localEulerAngles;

        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, targetAngle.x + 150f, smooth * Time.deltaTime),
            Mathf.LerpAngle(currentAngle.y, targetAngle.y - 30f, smooth * Time.deltaTime),
            Mathf.LerpAngle(currentAngle.z, targetAngle.z, smooth * Time.deltaTime));


        Artnet_currentAngle = new Vector3(
            Mathf.LerpAngle(Artnet_currentAngle.x, targetAngle.x, smooth * Time.deltaTime),
            Mathf.LerpAngle(Artnet_currentAngle.y, targetAngle.y, smooth * Time.deltaTime),
            Mathf.LerpAngle(Artnet_currentAngle.z, targetAngle.z, smooth * Time.deltaTime));


        Light.transform.localEulerAngles = currentAngle;
        test_cube.transform.localEulerAngles = currentAngle;

        // ArtNet.instance.Light_Y = new_Y;
        // ArtNet.instance.Light_X = new_X;
    }
}

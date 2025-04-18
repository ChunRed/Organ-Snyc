using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.XR;

public class RecenterOrigin : MonoBehaviour
{


    public Transform head;
    public Transform origin;
    public Transform target;
    public Transform target_2;
    public Transform target_3;

    public Material VR_CenterIcon;
    public Material LAMP_CenterIcon;

    public bool Move_flag = false;
    [Header("移動速度")]
    [Range(0.0001f, 1f)]
    public float Move_speed = 0.01f;
    [Header("移動加速度")]
    public AnimationCurve Move_curve;
    public float easing = 0f;

    float current_percent = 0f;


    //controllers操作變數
    private InputDevice rightController;
    

    public void Recenter(){
        Vector3 offset = head.position - origin.position;
        offset.y = 0.4f;
        origin.position = target.position - offset;

        Vector3 targetForward = target.forward;
        targetForward.y = 0f;

        Vector3 cameraForward = head.forward;
        cameraForward.y = 0;

        float angle = Vector3.SignedAngle(cameraForward, targetForward, Vector3.up);

        origin.RotateAround(head.position, Vector3.up, angle);
    }



    public void MoveTo(Transform Target, float Move_Speed){
        Vector3 offset = head.position - origin.position;
        offset.y = 0.4f;
        Vector3 new_podition = Target.position - offset;


        //計算 CURVE 加速度
        easing = Move_curve.Evaluate((target.position.z-origin.position.z) / (target.position.z-Target.position.z));


        origin.position = Vector3.Lerp(origin.position, new_podition, easing * Move_Speed * Time.deltaTime);
    }



    // 找到右手的控制器
    void GetController(){
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, inputDevices);

        if (inputDevices.Count > 0)
        {
            rightController = inputDevices[0];
        }

        if(MainPipeLine.instance.State == 1f){
            CancelInvoke("GetComponent");
        }
    }



    void Start()
    {
        Recenter();
        InvokeRepeating("GetController", 1f, 1f);
    }


    

    void Update()
    {
        if (Input.GetKey("r")) {
            Recenter();
            VR_CenterIcon.SetFloat("_pass", 1f);
            LAMP_CenterIcon.SetFloat("_pass", 1f);
        }
        else if(Input.GetKey("5") || MainPipeLine.instance.VR_isMove){
            Move_flag = true;
        }
        else if(Input.GetKey("t")){
            MoveTo(target_2, 0.5f);
        }


        if(Move_flag == true){
            MoveTo(target_2, Move_speed);
        }


        //手把控制
        if (rightController.isValid)
        {
            // A 按鈕（通常是primaryButton）
            bool aButtonPressed;
            if (rightController.TryGetFeatureValue(CommonUsages.primaryButton, out aButtonPressed) && aButtonPressed)
            {
                //Debug.Log("A 按鈕被按下");
            }

            // Trigger
            float triggerValue;
            if (rightController.TryGetFeatureValue(CommonUsages.trigger, out triggerValue) && triggerValue > 0.5f)
            {
                Recenter();
                VR_CenterIcon.SetFloat("_pass", 1f);
                LAMP_CenterIcon.SetFloat("_pass", 1f);
            }
            else{
                VR_CenterIcon.SetFloat("_pass", 0f);
                LAMP_CenterIcon.SetFloat("_pass", 0f);
            }

            // Grip
            float gripValue;
            if (rightController.TryGetFeatureValue(CommonUsages.grip, out gripValue) && gripValue > 0.1f)
            {
                //Debug.Log($"Grip 被按壓，數值: {gripValue}");
            }
        }

    }







    float Remap (float value, float from1, float to1, float from2, float to2) {
        if(value < from1) value = from1;
        if(value > to1) value = to1;
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}

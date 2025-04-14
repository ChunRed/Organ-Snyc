using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// 封裝震動功能並提供公開方法的控制器甦醒管理元件（OpenXR）。
/// </summary>



[DisallowMultipleComponent]
public class ControllerKeepAlive : MonoBehaviour
{
    [Header("設定參數")]
    [Tooltip("送出保持甦醒訊號的間隔秒數（建議5~15秒）")]
    [Range(1f, 30f)]
    public float keepAliveInterval = 10f;

    [Tooltip("震動強度，建議保持在 0.01 以防感知")]
    [Range(0f, 1f)]
    public float hapticAmplitude = 0.01f;

    [Tooltip("震動持續時間（秒），建議不超過 0.05 秒")]
    [Range(0.01f, 0.1f)]
    public float hapticDuration = 0.05f;

    [Header("執行狀態")]
    [Tooltip("是否自動定時保持控制器甦醒")]
    public bool autoKeepAlive = true;

    private InputDevice rightController;
    private float timer = 0f;

    void Start()
    {
        TryInitializeControllers();
    }

    void Update()
    {
        if (!rightController.isValid)
        {
            TryInitializeControllers();
        }

        if (!autoKeepAlive) return;

        timer += Time.deltaTime;
        if (timer >= keepAliveInterval)
        {
            SendKeepAlivePulse();
            timer = 0f;
        }
    }

    void TryInitializeControllers()
    {
        var leftDevices = new List<InputDevice>();
        var rightDevices = new List<InputDevice>();

        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftDevices);
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightDevices);

        if (rightDevices.Count > 0) rightController = rightDevices[0];
    }




    /// <summary>
    /// 對左右控制器發送一個微弱的震動訊號，用來保持控制器活躍。
    /// </summary>
    public void SendKeepAlivePulse()
    {
        if (rightController.isValid)
            rightController.SendHapticImpulse(0u, hapticAmplitude, hapticDuration);
    }



    /// <summary>
    /// 手動關閉自動保持功能。
    /// </summary>
    public void StopAutoKeepAlive()
    {
        autoKeepAlive = false;
    }



    /// <summary>
    /// 手動開啟自動保持功能。
    /// </summary>
    public void StartAutoKeepAlive()
    {
        autoKeepAlive = true;
        timer = 0f;
    }
}

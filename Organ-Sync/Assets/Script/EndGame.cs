using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [Header("UI材質")]
    public Material M_EndGame;
    float EndGame_pass = 0f;
    public bool showEnd = false;


    void Start()
    {
        M_EndGame.SetFloat("_pass", 0f);
    }

    
    void Update()
    {
        if(showEnd){
            EndGame_pass = Mathf.Lerp(EndGame_pass, 1f, 0.1f * Time.deltaTime);
            M_EndGame.SetFloat("_pass", EndGame_pass);
            if(EndGame_pass >= 1f){
                showEnd = false;
            }
        }
    }

}

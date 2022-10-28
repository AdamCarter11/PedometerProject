using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Android;

public class Steps : MonoBehaviour
{
    
    [SerializeField] TextMeshProUGUI stepDisplayText, workingDisplay;
    // Start is called before the first frame update
    void Start()
    {
        
        if (!Permission.HasUserAuthorizedPermission("android.permission.ACTIVITY_RECOGNITION"))
        {
            Permission.RequestUserPermission("android.permission.ACTIVITY_RECOGNITION");
            //
        }
        
        InputSystem.EnableDevice(StepCounter.current);
        if(StepCounter.current.enabled){
            workingDisplay.text = "Working: True";
        }


        //times per second it is updated
        //StepCounter.current.samplingFrequency = 5;
    }

    void FixedUpdate()
    {
        if(StepCounter.current.enabled)
        {
            stepDisplayText.text = ($"Steps: {StepCounter.current.stepCounter.ReadValue()}");
        }
    }
}

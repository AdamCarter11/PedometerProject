using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Android;

public class Steps : MonoBehaviour
{
    
    [SerializeField] TextMeshProUGUI stepDisplayText, workingDisplay;
    float lastStepVal;
    DateTime currTime;
    DateTime startTime; 
    int lastSteps = 0;
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

        if(!PlayerPrefs.HasKey("timeSaved")){
            currTime = DateTime.Now;
            //print(currTime);
            PlayerPrefs.SetString("timeSaved", currTime.ToString());

            //used for reseting steps at beginning of each day
            if(StepCounter.current.enabled){
                lastStepVal = StepCounter.current.stepCounter.ReadValue();
                PlayerPrefs.SetInt("lastStepVal", lastSteps);
            }
        }
    }

    private void Update() {

        if(StepCounter.current.enabled)
        {
            int tempSteps = PlayerPrefs.GetInt("lastSteps");
            stepDisplayText.text = ($"Steps: {StepCounter.current.stepCounter.ReadValue() - tempSteps}");
        }

        string originTime = PlayerPrefs.GetString("timeSaved");
        if(DateTime.Now.ToString()[3] != originTime[3]){
            //new day (and save time)
            currTime = DateTime.Now;
            //print(currTime);
            PlayerPrefs.SetString("timeSaved", currTime.ToString());

            //reset steps
            lastSteps = StepCounter.current.stepCounter.ReadValue();
            PlayerPrefs.SetInt("lastSteps", lastSteps);
        }
    }
}

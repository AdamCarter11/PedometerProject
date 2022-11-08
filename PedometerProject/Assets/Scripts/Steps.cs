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
    
    [SerializeField] TextMeshProUGUI stepDisplayText, workingDisplay, dateDisplay, versionText;
    float lastStepVal;
    DateTime currTime;
    DateTime startTime; 
    int lastSteps = 0;
    [HideInInspector] public static int totalSteps;
    // Start is called before the first frame update
    void Start()
    {
        print(PlayerPrefs.GetString("timeSaved"));
        if (!Permission.HasUserAuthorizedPermission("android.permission.ACTIVITY_RECOGNITION"))
        {
            Permission.RequestUserPermission("android.permission.ACTIVITY_RECOGNITION");
            //
        }
        
        InputSystem.EnableDevice(StepCounter.current);
        if(StepCounter.current.enabled){
            workingDisplay.text = "Working: True";
            if(!PlayerPrefs.HasKey("lastSteps")){
                PlayerPrefs.SetInt("lastSteps", 0);
            }
        }
        
        if(!PlayerPrefs.HasKey("timeSaved")){
            currTime = DateTime.Now;
            //print(currTime);
            PlayerPrefs.SetString("timeSaved", currTime.ToString());

            //used for reseting steps at beginning of each day
            /*
            if(StepCounter.current.enabled){
                lastStepVal = StepCounter.current.stepCounter.ReadValue();
                PlayerPrefs.SetInt("lastSteps", (int)lastStepVal);
            }
            */
        }
    }

    private void Update() {
        
        if(StepCounter.current.enabled)
        {
            int tempSteps = PlayerPrefs.GetInt("lastSteps");
            if(tempSteps <= 0){
                tempSteps = 0;
                PlayerPrefs.SetInt("lastSteps", 0);
            }
            
            totalSteps = StepCounter.current.stepCounter.ReadValue() - tempSteps;
            stepDisplayText.text = ($"Steps: {totalSteps}");
        }

        string originTime = PlayerPrefs.GetString("timeSaved");
        int lastStepVar = PlayerPrefs.GetInt("lastSteps");
        dateDisplay.text = "origin time: " + originTime + " last steps: " + lastStepVar;
        versionText.text = DateTime.Now.ToString();
        if(DateTime.Now.ToString()[3] != originTime[3] || DateTime.Now.ToString()[4] != originTime[4]){
            //reset steps
            lastSteps = StepCounter.current.stepCounter.ReadValue();
            PlayerPrefs.SetInt("lastSteps", lastSteps);

            //new day (and save time)
            currTime = DateTime.Now;
            //print(currTime);
            PlayerPrefs.SetString("timeSaved", currTime.ToString());
        }
    }
}

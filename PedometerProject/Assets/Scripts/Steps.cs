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
    int tempVal;
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

        if(StepCounter.current.enabled){
            lastStepVal = StepCounter.current.stepCounter.ReadValue();
        }

        if(!PlayerPrefs.HasKey("timeSaved")){
            currTime = DateTime.Now;
            //print(currTime);
            PlayerPrefs.SetString("timeSaved", currTime.ToString());
        }
        //times per second it is updated
        //StepCounter.current.samplingFrequency = 5;
    }

    void FixedUpdate()
    {
        
    }
    private void Update() {

        if(StepCounter.current.enabled)
        {
            if(lastStepVal <= StepCounter.current.stepCounter.ReadValue()-lastSteps){
                //lerp and change the lastStepVal
                //lerps the step value so its not so abrupt
                
                tempVal = (int)Mathf.Lerp(lastStepVal, StepCounter.current.stepCounter.ReadValue() - lastSteps, Time.deltaTime / 2);
                
                //updates text
                stepDisplayText.text = ($"Steps: {tempVal}");

                //updates lastStepVal if it caught up
                if(tempVal == StepCounter.current.stepCounter.ReadValue() - lastSteps){
                    lastStepVal = tempVal;
                    tempVal = 0;
                }
            }
            
        }

        string originTime = PlayerPrefs.GetString("timeSaved");
        if(DateTime.Now.ToString()[3] != originTime[3]){
            //new day (and save time)
            currTime = DateTime.Now;
            //print(currTime);
            PlayerPrefs.SetString("timeSaved", currTime.ToString());

            //reset steps
            lastSteps = StepCounter.current.stepCounter.ReadValue();
        }
    }
}

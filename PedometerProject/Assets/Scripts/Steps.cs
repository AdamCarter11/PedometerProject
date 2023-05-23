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
    DateTime currTime;
    int lastSteps = 0;
    [HideInInspector] public static int totalSteps; // this is public so we can access it from other scripts if needed
    bool delayed = true;

    void Start()
    {
        // request permissions to use step counter on phone if it hasn't be approved yet
        if (!Permission.HasUserAuthorizedPermission("android.permission.ACTIVITY_RECOGNITION"))
        {
            Permission.RequestUserPermission("android.permission.ACTIVITY_RECOGNITION");
        }
        
        // enables step counter once it's been approved and sets laststeps if it doesn't have a value yet
        InputSystem.EnableDevice(StepCounter.current);
        if(StepCounter.current.enabled){
            workingDisplay.text = "Working: True";
            if(!PlayerPrefs.HasKey("lastSteps")){
                PlayerPrefs.SetInt("lastSteps", 0);
            }
        }
        
        // saves the time saved as a baseline to compare when a new day has passed
        if(!PlayerPrefs.HasKey("timeSaved")){
            currTime = DateTime.Now;
            PlayerPrefs.SetString("timeSaved", currTime.ToString());
        }

        // adding this delayed start to see if it gives the app time to pick up the android permissions + steps
        StartCoroutine(delayedStart());
    }

    IEnumerator delayedStart()
    {
        yield return new WaitForSeconds(.2f);
        delayed = false;
    }

    private void Update() {
        // everything relying on steps should be below this check
        if (delayed) return;

        // gets steps from the android pedometer and displays them in a text box
        calculateStepsFunc();

        // resets steps on each new day
        resetStepsFunc();
    }

    void calculateStepsFunc()
    {
        if (StepCounter.current.enabled)
        {
            int tempSteps = PlayerPrefs.GetInt("lastSteps");
            if (tempSteps <= 0)
            {
                tempSteps = 0;
                PlayerPrefs.SetInt("lastSteps", 0);
            }

            // tempSteps is the steps up to this point and is used to reset total steps for each day
            //Android doesn't reset the pedometer until the phones been reset
            totalSteps = StepCounter.current.stepCounter.ReadValue() - tempSteps;
            stepDisplayText.text = ($"Steps: {totalSteps}");
        }
    }

    void resetStepsFunc()
    {
        string originTime = PlayerPrefs.GetString("timeSaved");
        if (DateTime.Now.Date != DateTime.Parse(originTime).Date)
        {
            // saves the current steps on a new day which will then be subtracted from all future days (on a new day, it should start at 0 because steps-steps = 0)
            lastSteps = StepCounter.current.stepCounter.ReadValue();
            PlayerPrefs.SetInt("lastSteps", lastSteps);

            // new day (and save time)
            currTime = DateTime.Now;
            PlayerPrefs.SetString("timeSaved", currTime.ToString());
        }
    }
}

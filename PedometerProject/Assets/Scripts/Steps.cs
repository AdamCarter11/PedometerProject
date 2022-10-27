using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class Steps : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI stepDisplayText, workingDisplay;
    // Start is called before the first frame update
    void Start()
    {
        /*
        if (!Permission.HasUserAuthorizedPermission("android.permission.ACTIVITY_RECOGNITION"))
        {
            Permission.RequestUserPermission("android.permission.ACTIVITY_RECOGNITION");
        }
        */
        InputSystem.EnableDevice(StepCounter.current);
        if(StepCounter.current.enabled){
            workingDisplay.text = "Working: True";
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(StepCounter.current.enabled)
        {
            stepDisplayText.text = ($"Steps: {StepCounter.current.stepCounter.ReadValue()}");
        }
    }
}

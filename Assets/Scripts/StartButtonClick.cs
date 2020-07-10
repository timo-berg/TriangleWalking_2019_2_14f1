using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;
using TMPro;
using UnityEngine.VR;
       
public class StartButtonClick : Singleton<StartButtonClick>
{
    public string ID;
    public string trial;
    public bool isVR;
    
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.XR.XRSettings.enabled = false;
        isVR = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onToggle(){
        isVR = !isVR;
    }

    public void LoadExperiment() {
        TMP_InputField IDInput = GameObject.Find("IDInput").GetComponent<TMP_InputField>();
        ID = IDInput.text;
        TMP_InputField TrialInput = GameObject.Find("TrialInput").GetComponent<TMP_InputField>();
        trial = TrialInput.text;
        GameObject startManager = GameObject.Find("Managers");
        DontDestroyOnLoad (startManager);
        SceneManager.LoadScene("MainExperiment");
        Debug.Log(isVR);
        if (isVR) {
            UnityEngine.XR.XRSettings.enabled = true;
        } else {
            UnityEngine.XR.XRSettings.enabled = false;
        }
        
    }
}

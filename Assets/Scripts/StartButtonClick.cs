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
    public string height;
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
        Debug.Log(isVR);
    }

    public void LoadExperiment() {
        TMP_InputField IDInput = GameObject.Find("IDInput").GetComponent<TMP_InputField>();
        ID = IDInput.text;
        TMP_InputField HeightInput = GameObject.Find("HeightInput").GetComponent<TMP_InputField>();
        height = HeightInput.text;
        GameObject startManager = GameObject.Find("_StartButtonManager");
        DontDestroyOnLoad (startManager);
        SceneManager.LoadScene("MainExperiment");
        if (isVR) {
            UnityEngine.XR.XRSettings.enabled = true;
        }
        
    }
}

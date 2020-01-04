using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;
using TMPro;
       
public class StartButtonClick : MonoBehaviour
{
    //public TMP_InputField IDInput;
    //public InputField HeightInput;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadExperiment() {
        TMP_InputField IDInput = GameObject.Find("IDInput").GetComponent<TMP_InputField>();
        string id = IDinput.text;
        TMP_InputField HeightInput = GameObject.Find("HeightInput").GetComponent<TMP_InputField>();
        string height = HeightInput.text;
        
        SceneManager.LoadScene("MainExperiment");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSensitivity : MonoBehaviour {

    private string nameParam;
    public string NameParam { get { return nameParam; } }

    private Button applyButton;
    private float prefValue;
    private Slider slider;
    public Slider Slider { get { return slider; } }

    public void SetSens(float sliderValue)
    {
        //Debug.Log("SetSens(): " + sliderValue);
        if (prefValue != slider.value)
        {
            //Debug.Log("Turn On Apply Button");
            applyButton.interactable = true;
        }
        slider.value = sliderValue;
    }

    // Use this for initialization
    void Start()
    {
        nameParam = GameObject.FindObjectOfType<Settings>().SensitivityParam;
        applyButton = GameObject.Find("ApplyButton").GetComponent<Button>();
        slider = GetComponent<Slider>();
        prefValue = PlayerPrefs.GetFloat(nameParam, 0);
        slider.value = prefValue;
    }

    // Update is called once per frame
    void Update () {
		
	}
}

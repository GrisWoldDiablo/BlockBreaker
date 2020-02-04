using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SetGFX : MonoBehaviour {

    [SerializeField] private Text qualityText;
    private Button applyButton;
    private string[] gfxNames;
    private float prefValue;
    public float PrefValue { get { return prefValue; } set { prefValue = value; } }
    private Slider slider;
    public Slider Slider { get { return slider; } }


    public void SetQuality(float sliderValue)
    {
  
        int gfxIndex = (int)Mathf.Floor(sliderValue);
        qualityText.text = gfxNames[gfxIndex];
        if (prefValue != slider.value)
        {
            //Debug.Log("Turn On Apply Button");
            applyButton.interactable = true;
        }
        slider.value = sliderValue;
    }

	// Use this for initialization
	void Start () {
        applyButton = GameObject.Find("ApplyButton").GetComponent<Button>();
        slider = GetComponent<Slider>();
        gfxNames = QualitySettings.names;
        slider.value = prefValue;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

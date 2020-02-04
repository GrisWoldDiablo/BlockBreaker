using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ReturnSettings : MonoBehaviour {

    [SerializeField] private int confirmationPanelIndex;
    private MenuInteraction menuCode;
    private Button applyButton;

    public void ExitSettings(int panelIndex)
    {
        if (!applyButton.interactable)
        {
            menuCode.PanelToggle(panelIndex);
        }
        else
        {
            menuCode.PanelToggle(confirmationPanelIndex);
        }
    }

	// Use this for initialization
	void Start () {
        applyButton = GameObject.Find("ApplyButton").GetComponent<Button>();
        menuCode = GameObject.FindObjectOfType<MenuInteraction>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

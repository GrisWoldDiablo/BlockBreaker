using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonToInputField : MonoBehaviour {

    [SerializeField] private InputField inputField;
    private string letter;
   
    public void SendLetter()
    {
        inputField.text += letter;
    }

	// Use this for initialization
	void Start () {
        letter = this.name[0].ToString();
	}

	// Update is called once per frame
	void Update () {
		
	}
}

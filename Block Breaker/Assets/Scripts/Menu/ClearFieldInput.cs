using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearFieldInput : MonoBehaviour {

    [SerializeField] private InputField inputField;

    public void ClearField()
    {
        inputField.text = string.Empty;
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

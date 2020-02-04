using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectableInteraction : MonoBehaviour,IPointerEnterHandler,IDeselectHandler,ISelectHandler {

	private bool selected = false;
	public bool Selected { get { return selected; } }

	public void OnPointerEnter(PointerEventData eventData)
	{
		GetComponent<Selectable>().Select();
	}

	public void OnDeselect(BaseEventData eventData)
	{
		GetComponent<Selectable>().OnPointerExit(null);
		selected = false;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnSelect(BaseEventData eventData)
	{
		selected = true;
	}
}

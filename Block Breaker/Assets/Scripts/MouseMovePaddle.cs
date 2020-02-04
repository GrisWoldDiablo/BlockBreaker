using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseMovePaddle : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {

    private MovePaddle paddleCode;
    private Vector3 pointerPosition;
    private Image playPanelImage;
    private Camera mainCamera;
    private float currentTime;
    private float delayTime = 0.5f;
    private bool firstClickReady = true;
    private MoveBall ballCode;
    private GameManager code;

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            playPanelImage.rectTransform, eventData.position, mainCamera, out pointerPosition);
        paddleCode.Pos = pointerPosition.x;
    }

#if UNITY_ANDROID ||  UNITY_WEBGL || UNITY_STANDALONE
    public void OnPointerDown(PointerEventData eventData)
    {

        if (firstClickReady)
        {
            currentTime = Time.time;
            firstClickReady = false;
            Debug.Log("CLICK ONCE");
        }
        else
        {
            if (Time.time <= currentTime + delayTime)
            {
                code.PaddleFire();
                code.BallCode.PaddleFire();
                //RectTransformUtility.ScreenPointToWorldPointInRectangle(
                //   playPanelImage.rectTransform, eventData.position, mainCamera, out pointerPosition);
                //paddleCode.Pos = pointerPosition.x;
                Debug.Log("CLICK TWICE");
                firstClickReady = true;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (code.CurrentPower == PowerTypes.GlueBall)
        {
            code.BallCode.PaddleFire();
        }
    }

#endif


    // Use this for initialization
    void Start () {
        code = GameObject.Find("GameManager").GetComponent<GameManager>();
        paddleCode = GameObject.Find("Paddle").GetComponent<MovePaddle>();
        playPanelImage = GameObject.Find("PlayPanel").GetComponent<Image>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Time.time > currentTime + delayTime && !firstClickReady)
        {
            Debug.Log("TOO LATE");
            firstClickReady = true;
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wooble : MonoBehaviour
{

    [SerializeField] private float speed = 0.1f;
    [SerializeField] private bool toTheRight = true;
    private Vector3 OgPos, upPos, rightPos, leftPos;
    private float smoothTime = 0.1f;
    private bool moveUP = true;
    private bool moveRight = true;

    // Use this for initialization
    void Start()
    {
        OgPos = transform.position;
        upPos = transform.position + Vector3.up * 0.3f;
        rightPos = transform.position + Vector3.right * 4;
        leftPos = transform.position + Vector3.left * 4;
        transform.position = Vector3.Lerp(OgPos, rightPos, smoothTime * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        smoothTime = speed * Time.deltaTime;
        if (toTheRight)
        {
            if (transform.position.x < rightPos.x && moveRight)
            {
                transform.position = transform.position + Vector3.right * smoothTime;
            }
            else
            {
                moveRight = false;
                transform.position = transform.position + Vector3.left * smoothTime;
                if (transform.position.x < OgPos.x)
                {
                    moveRight = true;
                }
            }
        }
        else
        {
            if (transform.position.x > leftPos.x && moveRight)
            {
                transform.position = transform.position + Vector3.left * smoothTime;
            }
            else
            {
                moveRight = false;
                transform.position = transform.position + Vector3.right * smoothTime;
                if (transform.position.x > OgPos.x)
                {
                    moveRight = true;
                }
            }
        }

        if (transform.position.y < upPos.y && moveUP)
        {
            transform.position = transform.position + Vector3.up * smoothTime;
        }
        else
        {
            moveUP = false;
            transform.position = transform.position + Vector3.down * smoothTime;
            if (transform.position.y < OgPos.y)
            {
                moveUP = true;
            }
        }

    }
}

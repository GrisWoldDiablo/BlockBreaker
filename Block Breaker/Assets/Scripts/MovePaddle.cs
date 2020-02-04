using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePaddle : MonoBehaviour
{
    [SerializeField] private float launchAngleMultiplier = 1.0f;
    [SerializeField] private Slider slider;
    [SerializeField] private float positionLimit = 2.0f;
    private GameManager code;
    
    private Vector3 aimLineOrigin;
    private Vector3 aimLineDirection;
    public Vector3 AimLineDirection { get { return aimLineDirection; } }

    private LineRenderer aimLine;
    public LineRenderer AimLine { get { return aimLine; } set { aimLine = value; } }

    

    private float pos = 0;
    public float Pos { get { return pos; } set { pos = value; } }

    // Use this for initialization
    void Start() {
        code = GameObject.Find("GameManager").GetComponent<GameManager>();
        aimLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update() {
        
        float inputAxis = Input.GetAxis("Horizontal");

        if (Input.GetButton("PaddleFire"))
        {
            pos += inputAxis * Time.deltaTime * 2 * code.PaddleSensitivity;
        }
        else
        {
            pos += inputAxis * Time.deltaTime * code.PaddleSensitivity;
        }

        if (pos > positionLimit)
        {
            pos = positionLimit;
        }
        else if (pos < -positionLimit)
        {
            pos = -positionLimit;
        }
        transform.position = new Vector3(pos, transform.position.y, transform.position.z);
        

        aimLineOrigin = transform.position + code.BallCode.GluePosition;
        aimLineDirection = (Vector3.up) + (Vector3.right * pos * launchAngleMultiplier);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(aimLineOrigin, aimLineDirection);
        if (aimLineDirection.x < -2.6f)
        {
            aimLineDirection.x = -2.6f;
        }
        else if (aimLineDirection.x > 2.6f)
        {
            aimLineDirection.x = 2.6f;
        }
        DrawAimLine(new Vector3[] { aimLineOrigin, raycastHit2D.point });
    }

    public void DrawAimLine(Vector3[] aimLinePositions)
    {
        aimLine.SetPositions(aimLinePositions);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Power"))
        {
            code.AddPoints(col.GetComponent<PowerBehavior>().PowerValue, false);
            code.SetPower(col.GetComponent<PowerBehavior>().PowerType);
            Destroy(col.gameObject);
        }
    }


}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if (UNITY_EDITOR)
[ExecuteInEditMode]
public class ExecInEditor : MonoBehaviour
{
    void Awake()
    {
        //GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.3f, 0.9f), Random.Range(0.3f, 0.9f), Random.Range(0.3f, 0.9f));
        GetComponent<SpriteRenderer>().flipX = Random.Range(0, 2) == 0 ? false : true;
        GetComponent<SpriteRenderer>().flipY = Random.Range(0, 2) == 0 ? false : true;
    }
}
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpecifics : MonoBehaviour
{
    [SerializeField] List<GameObject> androidShowObjects;
    [SerializeField] List<GameObject> androidHideObjects;
    [SerializeField] List<GameObject> webglShowObjects;
    [SerializeField] List<GameObject> webglHideObjects;
    
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID

        foreach (var item in androidShowObjects)
        {
            item.SetActive(true);
        }
        foreach (var item in androidHideObjects)
        {
            item.SetActive(false);
        }
#else
        foreach (var item in androidShowObjects)
        {
            item.SetActive(false);
        }
        foreach (var item in androidHideObjects)
        {
            item.SetActive(true);
        }
#endif

#if UNITY_WEBGL
        foreach (var item in webglShowObjects)
        {
            item.SetActive(true);
        }
        foreach (var item in webglHideObjects)
        {
            item.SetActive(false);
        }
#else
        foreach (var item in webglShowObjects)
        {
            item.SetActive(false);
        }
        foreach (var item in webglHideObjects)
        {
            item.SetActive(true);
        }
#endif
    }
}

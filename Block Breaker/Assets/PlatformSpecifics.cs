using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpecifics : MonoBehaviour
{
    [SerializeField] List<GameObject> androidShowObjects;
    [SerializeField] List<GameObject> androidHideObjects;
    
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
    }
}

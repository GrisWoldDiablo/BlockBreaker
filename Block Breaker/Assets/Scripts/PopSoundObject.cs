using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PopSoundObject : MonoBehaviour {

    [SerializeField] private AudioClip[] burnClips;

    private void Awake()
    {
        GetComponent<AudioSource>().clip = burnClips[Random.Range(0, burnClips.Length)];
    }
    // Use this for initialization
    void Start () {
        GetComponent<AudioSource>().Play();
    }
	
	// Update is called once per frame
	void Update () {
        if (!GetComponent<AudioSource>().isPlaying)
        {
            Destroy(gameObject);
        }
	}
}

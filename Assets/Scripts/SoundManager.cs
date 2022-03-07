using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip hit;
    AudioSource audio;
    void Start()
    {
        hit = Resources.Load<AudioClip>("hit");
        audio = GetComponent<AudioSource>();
    }

    public void playHit()
    {
        audio.PlayOneShot(hit);
    }
}

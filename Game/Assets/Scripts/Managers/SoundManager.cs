using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SoundManager 
{
    AudioSource source = new AudioSource();
    public void Start()
    {
        GameObject root = GameObject.Find("@Sound");
        source = root.GetComponent<AudioSource>();
    }
    public void PlayEating()
    {
        source.clip = Resources.Load<AudioClip>("Sound/eating_sound");
        source.Play();
    }
}

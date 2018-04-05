using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager soundInstance;

    public List<AudioSource> audiosources = new List<AudioSource>();

    private void Awake()
    {
        if (soundInstance == null)
        {
            soundInstance = this;
        }
    }
}

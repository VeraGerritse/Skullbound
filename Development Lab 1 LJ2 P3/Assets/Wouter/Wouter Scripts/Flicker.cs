using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour {

    public Light myLight;
    public float defaultIntensity;
    
    public float max;
    public float min;

    void Start()
    {
        myLight = GetComponent<Light>();

        defaultIntensity = myLight.intensity;

        min = defaultIntensity * 0.9f;
        max = defaultIntensity * 1.1f;
    }

    void Update()
    {
        myLight.intensity = defaultIntensity * (Mathf.PingPong(Time.time * 3, 0.4f) + 1);

        

    }

}

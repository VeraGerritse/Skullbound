using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRoom : MonoBehaviour {
    public int index;

    private void OnTriggerEnter(Collider other)
    {
        Transform chamber = GetComponent<Transform>();
        foreach(Transform child in chamber)
        {
            child.gameObject.SetActive(true);
        }
    }
}

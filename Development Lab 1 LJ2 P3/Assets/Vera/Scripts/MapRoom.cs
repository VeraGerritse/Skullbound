using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRoom : MonoBehaviour {
    public int index;
    bool enteredRoom;

    private void OnTriggerStay(Collider other)
    {
        Transform chamber = GetComponent<Transform>();
        if (other.gameObject.tag == "Player" && !enteredRoom)
        {
            enteredRoom = true;
            foreach (Transform child in chamber)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}

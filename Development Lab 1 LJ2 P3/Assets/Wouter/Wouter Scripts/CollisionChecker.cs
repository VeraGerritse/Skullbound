using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour {
    public bool collide;
    public GameObject hitObject;

    public void OnTriggerStay(Collider other)
    {
        collide = true;
        hitObject = other.gameObject;
    }

    public void OnTriggerExit(Collider other)
    {
        collide = false;
        hitObject = null;
        
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour {

    public GameObject owner;

    public bool top;

    public void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (top)
            {
                owner.GetComponent<Animator>().SetTrigger("Sting");
            }
            else
            {
                owner.GetComponent<Animator>().SetTrigger("StingBack");
            }
        }       
    }




}

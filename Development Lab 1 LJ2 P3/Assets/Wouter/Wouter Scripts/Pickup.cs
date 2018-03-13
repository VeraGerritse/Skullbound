using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    public bool canBePickedUp;

    public GameObject owner;
    public bool followPlayer;
    private float timer;
    public Vector3 weaponslot;
    

    void Start()
    {
        //followPlayer = false;
    }

    /*
    public void PickMeUp()
    {
        if(GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().useGravity = false;
        }
        if(GetComponent<MeshCollider>() != null)
        {
            GetComponent<MeshCollider>().enabled = false;
        }
    }
    */


}

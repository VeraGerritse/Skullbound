using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuWalking : MonoBehaviour {

    public int speed;
    public Transform start;
            
	void Update () {
        int forward = 1 * speed / 10;
        transform.Translate(Vector3.forward * forward);
        
        print(forward);
        print(transform.position.z);
        if(transform.position.z > 1f)
        {
            transform.position = start.position;
        }
	}
}

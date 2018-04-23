using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour {

    public float health;

    public void Fall()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void TurnOffAnim()
    {
        GetComponent<Animator>().enabled = false;

        GetComponent<Rigidbody>().AddExplosionForce(2000, Camera.main.transform.position, 20);
        GetComponent<Rigidbody>().AddForce(Vector3.up * 2000);
    }

    public void Die()
    {
        Destroy(this.gameObject, 5f);
        GetComponent<Animator>().SetTrigger("Die");
    }
}


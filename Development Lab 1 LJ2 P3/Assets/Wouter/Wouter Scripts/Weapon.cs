using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public GameObject owner;
    public bool followPlayer;
    private float timer;
    public Vector3 weaponslot;

    //stats
    public string weaponName;
    public int itemId;
    public float attack;
    public float defence;
    public float staminacost;
  
    //enemy only

    void Start()
    {
        timer = 0;
        this.gameObject.SetActive(true);
        weaponslot = GameObject.Find("WeaponSlot").transform.position;
    }
    
    public void OnTriggerEnter(Collider other)       
    {
        if(owner != null)
        {
            if (owner.GetComponent<Enemy>() != null)
            {
                if (other.tag == "Player")
                {
                    if (!other.GetComponent<PlayerStats>().playerBLocks)
                    {
                        other.GetComponent<PlayerStats>().ChangeHealth(-attack + owner.GetComponent<Enemy>().extraAttack);
                        other.GetComponent<PlayerActions>().anim.SetTrigger("Recoil");
                    }
                    else
                    {
                        if (owner != null)
                        {
                            owner.GetComponent<Animator>().SetTrigger("Revert");
                            other.GetComponent<PlayerActions>().anim.SetTrigger("RecoilBlock");
                        }
                    }
                }
            }
        }        
    }
    
}

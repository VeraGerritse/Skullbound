using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public GameObject owner;
    public Animator ownerAnimator;
    public CombatAi myCombatAi;
    public bool hyper;
    public bool knockback;

    public bool isVisible;

    //stats
    public string weaponName;
    public int itemId;
    public float attack;
    public float staminacost;

    //enemy only


    private void Update()
    {
        if(Input.GetKeyDown("u"))
        {
            
        }


    }



    public void OnTriggerEnter(Collider other)       
    {
        if(true)
        {
            //print("1");
            if (true)
            {
                //print("2");
                if (other.tag == "Player")
                {
                    //print("3");
                    //if (!other.GetComponent<PlayerStats>().playerBLocks)

                    if (!other.GetComponent<PlayerStats>().playerBLocks)
                    {
                        //print("4");
                        other.GetComponent<PlayerStats>().ChangeHealth(-attack);

                        other.GetComponent<PlayerActions>().anim.SetTrigger("Recoil");
                        if(knockback)
                        {
                            other.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                            other.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 300 + Vector3.back * 500);
                        }
                    }
                    else
                    {

                        if (owner != null)
                        {
                            if(!hyper)
                            {
                                print("Should Recoil now");
                                GetComponentInParent(typeof(Animator)).transform.GetComponent<Animator>().SetTrigger("Revert");
                            }
                            
                            other.GetComponent<PlayerActions>().anim.SetTrigger("RecoilBlock");
                            SoundManager.soundInstance.audiosources[Random.Range(12, 15)].Play();
                            myCombatAi.actionCooldown = 2;
                            if (other.GetComponent<PlayerStats>().shield != null)
                            {
                                other.GetComponent<PlayerStats>().ChangeStamina(-attack / 2 * (1 - other.GetComponent<PlayerStats>().shield.GetComponent<Shield>().stability / 100));
                            }
                            else
                            {
                                other.GetComponent<PlayerStats>().ChangeStamina(-attack);
                            }
                        }
                    }
                }
            }
        }        
    }   
}

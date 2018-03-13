using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour {

    public Animator anim;
    public bool canCombo;
    public bool alternate;
    public float speedmodifier;

    public bool isNeutral;

    

    public CollisionChecker collisionChecker;

    public PlayerStats playerStats;

    private void Start()
    {
        Physics.IgnoreLayerCollision(10,10);
        anim = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if(playerStats.weapon != null)
        {
            //anim.SetTrigger("Draw");
        }

        if (speedmodifier < 1)
        {
            anim.SetFloat("TestFloat", speedmodifier);
            speedmodifier += Time.deltaTime * 5;
        }

        if(canCombo)
        {
            anim.SetBool("CanCombo", true);
        }
        else
        {
            anim.SetBool("CanCombo", false);
        }
        if(Input.GetKeyDown("r"))
        {
            //anim.SetTrigger("Draw");
        }

        if(Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Swing");
            anim.ResetTrigger("TestTrigger");
            anim.ResetTrigger("Potion");
        }

        if (Input.GetButtonDown("Fire2"))
        {
            anim.SetTrigger("Block");
            anim.ResetTrigger("UnBlock");
            anim.ResetTrigger("Potion");
        }

        if(Input.GetButtonUp("Fire2"))
        {
            anim.ResetTrigger("Swing");
        }

        if(!Input.GetButton("Fire2"))
        {
            anim.SetTrigger("UnBlock");
            anim.ResetTrigger("Potion");
        }

        if(Input.GetKeyDown("f"))
        {
            //anim.SetFloat("TestFloat", -1f);
            anim.SetTrigger("TestTrigger");
        }
        else
        {
            //anim.SetFloat("TestFloat", 1f);
        }

        if (Input.GetButton("Alternate"))
        {
            anim.SetBool("Alternate", true);
        }
        else
        {
            anim.SetBool("Alternate", false);
        }

        if (Input.GetKeyDown("q"))
        {
            anim.SetTrigger("Potion");
            anim.ResetTrigger("OpenDoor");
        }

        if(Input.GetKeyDown("m"))
        {
            anim.SetTrigger("Map");
            anim.ResetTrigger("OpenDoor");
            anim.ResetTrigger("Potion");
        }

        if(Input.GetKeyDown("e"))
        {
            if(anim.GetCurrentAnimatorStateInfo(2).IsTag("lol"))
            {
                //anim.SetTrigger("OpenDoor");
                //anim.ResetTrigger("Potion");
                anim.ResetTrigger("Swing");
                Interact();
            }
        }

        if(Input.GetKeyDown("p"))
        {       
            anim.SetTrigger("Die");
        }
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * 3, Color.red);
    }

    public void Interact()
    {   
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * 3, out hit, 3))
        {        
            //print("1");
            if (hit.transform.gameObject.GetComponent<Pickup>() != null)
            {
                //print("2");
                if (hit.transform.gameObject.GetComponent<Pickup>().canBePickedUp)
                {
                    //print("3");
                    if (hit.transform.gameObject.GetComponent<Weapon>() != null)
                    {
                        GameObject w = hit.transform.gameObject;
                        w.SetActive(false);
                        playerStats.previousWeapon = playerStats.weapon;
                        playerStats.weapon = hit.transform.gameObject;

                        GameObject spawn = Instantiate(playerStats.previousWeapon, hit.transform.position + new Vector3(0,1,0), playerStats.weapon.transform.rotation);
                        spawn.SetActive(true);
                        Destroy(playerStats.previousWeapon);
                        for (int i = 0; i < playerStats.viewmodelgear.Count; i++)
                        {
                            if (playerStats.viewmodelgear[i] != null)
                            {
                                playerStats.viewmodelgear[i].SetActive(false);
                            }

                            anim.SetTrigger("On");
                            playerStats.viewmodelgear[playerStats.weapon.GetComponent<Weapon>().itemId].SetActive(true);
                        }
                    }
                    else if(hit.transform.gameObject.GetComponent<Shield>() != null)
                    {
                        GameObject s = hit.transform.gameObject;
                        s.SetActive(false);
                        playerStats.previousShield = playerStats.shield;
                        playerStats.shield = hit.transform.gameObject;

                        GameObject spawn = Instantiate(playerStats.previousShield, hit.transform.position + new Vector3(0, 1, 0), playerStats.shield.transform.rotation);
                        spawn.SetActive(true);
                        Destroy(playerStats.previousShield);
                        for (int i = 0; i < playerStats.viewmodelgearLeft.Count; i++)
                        {
                            if (playerStats.viewmodelgearLeft[i] != null)
                            {
                                playerStats.viewmodelgearLeft[i].SetActive(false);
                            }

                            anim.SetTrigger("ShieldOn");
                            playerStats.viewmodelgearLeft[playerStats.shield.GetComponent<Shield>().itemId].SetActive(true);
                        }
                    }
                }
            }
        }
    }

    public void Hit(float amount)
    {
        if(collisionChecker.hitObject != null)
        {
            //speedmodifier = amount;
            if(collisionChecker.hitObject.GetComponent<Rigidbody>() != null)
            {
                collisionChecker.hitObject.GetComponent<Rigidbody>().AddForce(transform.forward * 100 + transform.up * 30);
            }

            if (collisionChecker.hitObject.GetComponent<Enemy>() != null)
            {
                if (collisionChecker.hitObject.GetComponent<Enemy>().isBlocking)
                {
                    anim.SetTrigger("TestTrigger");
                    //anim.ResetTrigger("TestTrigger");
                }
            }
            else if (collisionChecker.hitObject.tag == "Environment")
            {
                anim.SetTrigger("TestTrigger");
            }
        }
    }
    public void SetSpeed(float amount)
    {
        speedmodifier = amount;
    }
}

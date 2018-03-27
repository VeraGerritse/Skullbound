using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour {

    public Animator anim;
    public bool canCombo;
    public bool alternate;
    public float speedmodifier;
    public LayerMask layer;
    public bool isNeutral;
    public LayerMask attacklayer;

    

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
            anim.SetBool("BlockBool", false);
            anim.ResetTrigger("Potion");
        }
        else
        {
            anim.SetBool("BlockBool", true);
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
            if(playerStats.potionCount > 0)
            {
                anim.SetBool("HasPotion", true);
                
            }
            else
            {
                anim.SetBool("HasPotion", false);
            }
            anim.SetTrigger("Potion");
            
            
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

        if(Input.GetKeyDown(KeyCode.O))
        {       
            anim.SetTrigger("Die");
        }
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * 3, Color.red);
    }

    public void Interact()
    {   
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * 3, out hit, 3, layer))
        {
            if (hit.collider.gameObject.GetComponent<Door>() != null)
            {
                hit.collider.gameObject.GetComponent<Door>().anim.SetTrigger("Open");
            }
            if (hit.transform.gameObject.GetComponent<Pickup>() != null)
            {
                if (hit.transform.gameObject.GetComponent<Pickup>().canBePickedUp)
                {
                    if (hit.transform.gameObject.GetComponent<Weapon>() != null)
                    {
                        GameObject w = hit.transform.gameObject;
                        w.SetActive(false);

                        playerStats.previousWeapon = playerStats.weapon;
                        playerStats.weapon = hit.transform.gameObject;
                        //Destroy(playerStats.previousWeapon);
                        
                        if (playerStats.previousWeapon != null)
                        {
                            if (playerStats.previousWeapon.GetComponent<Rigidbody>())
                            {
                                GameObject spawn = Instantiate(playerStats.previousWeapon, hit.transform.position + new Vector3(0, 1, 0), playerStats.weapon.transform.rotation);
                                spawn.SetActive(true);
                                spawn.GetComponent<Rigidbody>().isKinematic = false;
                                spawn.GetComponent<Interactables>().IsAwake = true;
                            }
                        }

                        //Destroy(playerStats.previousWeapon);
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
                        Destroy(playerStats.previousShield);
                        playerStats.shield.name = playerStats.shield.GetComponent<Shield>().shieldName;
                        

                        if (playerStats.previousShield != null)
                        {
                            if (playerStats.previousShield.GetComponent<Rigidbody>())
                            {
                                GameObject spawnshield = Instantiate(playerStats.previousShield, hit.transform.position + new Vector3(0, 1, 0), playerStats.shield.transform.rotation);
                                spawnshield.SetActive(true);
                                spawnshield.GetComponent<Rigidbody>().isKinematic = false;
                                spawnshield.GetComponent<Interactables>().IsAwake = true;
                            }
                        }

                        //Destroy(playerStats.previousShield);
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
                    else if(hit.transform.gameObject.GetComponent<Potion>() != null)
                    {
                        playerStats.potionCount++;
                        DestroyImmediate(hit.transform.gameObject);
                    }
                }
            }
        }
    }

    public void Hit(float amount)
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * 3, out hit, 2, attacklayer))
        {
            if (hit.transform.gameObject.GetComponent<Rigidbody>() != null)
            {
                hit.transform.GetComponent<Rigidbody>().AddForce(transform.forward * 400 + transform.up * 200);
            }
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.GetComponent<CombatAi>().ChangeHealth(-playerStats.weapon.GetComponent<Weapon>().attack);
            }
        }
    }
    public void ConsumePotion()
    {
        playerStats.ChangeHealth(playerStats.playerMaxHealth - playerStats.playerHealth);
        
        playerStats.potionCount--;
    }

    public void SetSpeed(float amount)
    {
        speedmodifier = amount;
    }
}

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
    public Canvas ui;

    public GameObject deactivatethis;

    public static bool staticplayerAttacks;
    public bool playerAttacks;

    public bool armed;
    public bool wait;
    

    public float powerTimer;



    public CollisionChecker collisionChecker;

    public PlayerStats playerStats;

    private void Start()
    {
        Physics.IgnoreLayerCollision(10,10);
        anim = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        PotionUI.instance.UpdateSyringe(playerStats.boostCount);
        PotionUI.instance.UpdatePotions(playerStats.potionCount);
        
    }

    public void ToggleUi()
    {
        ui = UIManager.instance.interfaceGame.gameObject.GetComponent<Canvas>();
        if (ui.enabled == true)
        {
            ui.enabled = false;
        }
        else
        {
            ui.enabled = true;
        }
    }

    public void PlayStep()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hit, 1.1f))
        {
            if (hit.transform.gameObject != null && hit.collider.gameObject.tag != "Player")
            {
                SoundManager.soundInstance.audiosources[Random.Range(17, 21)].Play();

            }
        }
        
    }

    private void Update()
    {
        if(wait)
        {
            anim.SetFloat("WakeUpSpeed", 0);
            if(Input.GetButtonDown("Interact"))
            {
                wait = false;
                GameObject.FindWithTag("Destroy").SetActive(false);
                GameObject.FindWithTag("Destroy2").SetActive(false);
            }
        }
        else
        {
            anim.SetFloat("WakeUpSpeed", 1);
        }
        staticplayerAttacks = playerAttacks;

        if(canCombo)
        {
            anim.ResetTrigger("Revert");
        }
        if(Input.GetButtonDown("Inject"))
        {
            if (playerStats.boostCount > 0)
            {
                anim.SetBool("HasBoost", true);

            }
            else
            {
                anim.SetBool("HasBoost", false);
            }
            anim.SetTrigger("Inject");
        }



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
            anim.ResetTrigger("PowerAttack");
            powerTimer = 0;
            if(playerStats.playerStamina > 0)
            {
                anim.SetTrigger("Swing");
                anim.ResetTrigger("TestTrigger");
                anim.ResetTrigger("Potion");
            }
        }
        if(Input.GetButtonUp("Fire1"))
        {
            powerTimer = 0;
        }
        if(Input.GetButton("Fire1"))
        {           
            powerTimer += Time.deltaTime;
            if(powerTimer >= 0.2f && playerStats.playerStamina > 0)
            {
                powerTimer = 0;
                anim.ResetTrigger("Swing");
                anim.SetTrigger("PowerAttack");
            }
        }

        if(playerStats.playerStamina <= 0)
        {
            anim.ResetTrigger("Swing");
        }

        if (playerStats.playerStamina <= 0)
        {
            anim.SetBool("BlockBool", false);
        }



        if (Input.GetButtonDown("Fire2") && playerStats.playerStamina <= 0)
        {
            anim.SetTrigger("Block");
            anim.ResetTrigger("UnBlock");
            anim.ResetTrigger("Potion");
            //SoundManager.soundInstance.audiosources[16].Play();
        }

        if(Input.GetButtonUp("Fire2"))
        {
            anim.ResetTrigger("Swing");
            

        }

        if(!Input.GetButton("Fire2"))
        {
            anim.SetBool("BlockBool", false);
            anim.ResetTrigger("Potion");
            //anim.ResetTrigger("Inject");
        }
        else if(playerStats.playerStamina > 1)
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
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * 2, Color.red);
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
                    SoundManager.soundInstance.audiosources[Random.Range(10, 12)].Play();
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
                    else if(hit.transform.gameObject.GetComponent<Potion>() != null && playerStats.potionCount != 3)
                    {
                        playerStats.potionCount++;
                        PotionUI.instance.UpdatePotions(playerStats.potionCount);
                        DestroyImmediate(hit.transform.gameObject);
                    }
                    else if(hit.transform.gameObject.GetComponent<BoostInjector>() != null && playerStats.boostCount != 3)
                    {
                        playerStats.boostCount++;
                        PotionUI.instance.UpdateSyringe(playerStats.boostCount);
                        DestroyImmediate(hit.transform.gameObject);
                    }
                }
            }
        }
    }

    public void PlayMiss()
    {
        SoundManager.soundInstance.audiosources[Random.Range(22, 26)].Play();
    }

    public void Hit(float amount)
    {
        
        playerStats.staminaRegenerationDelay = 1;
        playerStats.ChangeStamina(-playerStats.weapon.GetComponent<Weapon>().staminacost);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * 3, out hit, 2f, ~attacklayer)
            ||
            Physics.Raycast(Camera.main.transform.position + new Vector3(0.4f, 0, 0), Camera.main.transform.TransformDirection(Vector3.forward) * 3, out hit, 3f, ~attacklayer)
            ||
            Physics.Raycast(Camera.main.transform.position + new Vector3(0.4f, 0, 0), Camera.main.transform.TransformDirection(Vector3.forward) * 3, out hit, 3f, ~attacklayer)
            )

        {
            if (hit.transform.gameObject.GetComponent<Rigidbody>() != null)
            {
                print(hit.transform.name);

                if (hit.transform.tag == "Enemy")
                {
                    if(!hit.transform.GetComponent<CombatAi>().block)
                    {
                        hit.transform.GetComponent<CombatAi>().ChangeHealth(-playerStats.weapon.GetComponent<Weapon>().attack);
                    }
                    else
                    {
                        anim.ResetTrigger("Revert");
                        anim.SetTrigger("Revert");
                        SoundManager.soundInstance.audiosources[Random.Range(12, 16)].Play();
                        
                    }
                    
                }
                else
                {
                    hit.transform.GetComponent<Rigidbody>().AddForce(transform.forward * 400 + transform.up * 200);
                }
            }

        }
    }

    public void PowerAttack()
    {
        playerStats.staminaRegenerationDelay = 2;
        playerStats.ChangeStamina(-playerStats.weapon.GetComponent<Weapon>().staminacost * 3);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * 3, out hit, 4f, ~attacklayer) 
            ||
            Physics.Raycast(Camera.main.transform.position + new Vector3(1f,0,0), Camera.main.transform.TransformDirection(Vector3.forward) * 2, out hit, 4f, ~attacklayer)
            ||
            Physics.Raycast(Camera.main.transform.position + new Vector3(0.5f, 0, 0), Camera.main.transform.TransformDirection(Vector3.forward) * 2, out hit, 4f, ~attacklayer)
            ||
            Physics.Raycast(Camera.main.transform.position + new Vector3(-1f, 0, 0), Camera.main.transform.TransformDirection(Vector3.forward) * 2, out hit, 4f, ~attacklayer)
            ||
            Physics.Raycast(Camera.main.transform.position + new Vector3(-0.5f, 0, 0), Camera.main.transform.TransformDirection(Vector3.forward) * 2, out hit, 4f, ~attacklayer)
            )
        {
            if (hit.transform.gameObject.GetComponent<Rigidbody>() != null)
            {
                //print(hit.transform.name);

                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.GetComponent<CombatAi>().ChangeHealth(-playerStats.weapon.GetComponent<Weapon>().attack * 2);
                    hit.transform.GetComponent<Rigidbody>().AddForce(transform.forward * 300 + transform.up * 300);
                    
                }
                else
                {
                    hit.transform.GetComponent<Rigidbody>().AddForce(transform.forward * 1000 + transform.up * 500);
                }
            }

        }
    }

    public void Bash()
    {
        playerStats.staminaRegenerationDelay = 1;
        playerStats.ChangeStamina(-playerStats.shield.GetComponent<Shield>().bashcost);
        SoundManager.soundInstance.audiosources[16].Play();
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * 3, out hit, 1.75f, ~attacklayer)
            ||
            Physics.Raycast(Camera.main.transform.position + new Vector3(1f, 0, 0), Camera.main.transform.TransformDirection(Vector3.forward) * 3, out hit, 1.75f, ~attacklayer)
            ||
            Physics.Raycast(Camera.main.transform.position + new Vector3(0.5f, 0, 0), Camera.main.transform.TransformDirection(Vector3.forward) * 3, out hit, 1.75f, ~attacklayer)
            ||
            Physics.Raycast(Camera.main.transform.position + new Vector3(-1f, 0, 0), Camera.main.transform.TransformDirection(Vector3.forward) * 3, out hit, 1.75f, ~attacklayer)
            ||
            Physics.Raycast(Camera.main.transform.position + new Vector3(-0.5f, 0, 0), Camera.main.transform.TransformDirection(Vector3.forward) * 3, out hit, 1.75f, ~attacklayer))
        {
            if (hit.transform.gameObject.GetComponent<Rigidbody>() != null)
            {
                SoundManager.soundInstance.audiosources[Random.Range(12, 15)].Play();
                hit.transform.GetComponent<Rigidbody>().AddForce(transform.forward * 300 + transform.up * 150);
                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.GetComponent<CombatAi>().ChangeHealth(-playerStats.shield.GetComponent<Shield>().bashDamage);
                    hit.transform.GetComponent<CombatAi>().myAnimator.SetTrigger("Hurt");
                }
            }
        }
    }
    public void ConsumePotion()
    {
        playerStats.ChangeHealth(playerStats.playerMaxHealth - playerStats.playerHealth);       
        playerStats.potionCount--;
        PotionUI.instance.UpdatePotions(playerStats.potionCount);
    }

    public void SetSpeed(float amount)
    {
        speedmodifier = amount;
    }

    public void Shake()
    {
        anim.SetTrigger("Shake");
    }

    public void WaitForPickup()
    {
        wait = true;
    }

    public void DropBlade()
    {
        print("test");
        GameObject blade = GameObject.FindWithTag("Drop");
        //print(blade);
        //print(blade.GetComponent<Rigidbody>());
        blade.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        
    }
}

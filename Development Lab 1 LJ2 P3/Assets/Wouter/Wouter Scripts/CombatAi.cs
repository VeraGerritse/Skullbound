using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatAi : MonoBehaviour {

    public enum Task { Idle, Follow, Attack, Block, Spin, Smash };
    public Task task;


    public float speedbonus;

    public bool dualWieldWeapons;
    

    public Pathfinding myPathFinding;
    public Animator myAnimator;
    public Text textHP;
    public List<GameObject> bones = new List<GameObject>();
    public GameObject bonepieces;
    public RoomActivities myRoom;
    public AudioSource hurtSound;
    [Header("Stats")]
    public float Health;
    float maxHealth;

    public float actionCooldown;
    public bool dualwielder;
    public bool shielded;
    public bool block;
    public bool isBoss;

    public int nextattack;
    public bool canChoose;
    public bool isAttacking;
    public float choicetimer;

    private int oldSpeed;



    void Start()
    {
        maxHealth = Health;
        print(maxHealth);
        oldSpeed = myPathFinding.speed;
        textHP.text = Health.ToString();
        myPathFinding = GetComponent<Pathfinding>();
        myAnimator = transform.GetChild(0).GetComponent<Animator>();
        if(shielded)
        {
            myAnimator.SetBool("Shielded", true);
        }
        if(dualwielder)
        {
            myAnimator.SetBool("DualWield", true);
        }

        if(speedbonus == 0)
        {
            speedbonus = 1;
        }

        myAnimator.SetFloat("SpeedBonus", speedbonus);

        canChoose = true;
        

    }

    void Update()
    {

            

        if (shielded)
        {
            if(PlayerActions.staticplayerAttacks)
            {
                myAnimator.SetBool("Block", true);
                myPathFinding.speed = 0;
            }
            else
            {
                myAnimator.SetBool("Block", false);
                myPathFinding.speed = oldSpeed;
            }
            
        }
        
        

        choicetimer += Time.deltaTime;
        if(choicetimer >= 2)
        {

            if(isBoss)
            {
                myAnimator.SetBool("Smash", true);
                myAnimator.SetBool("Spin", false);
                myAnimator.SetBool("Attack", false);
                myAnimator.SetBool("Walk", false);
            }
            else
            {
                myAnimator.SetBool("Smash", false);
                myAnimator.SetBool("Spin", false);
                myAnimator.SetBool("Attack", false);
                myAnimator.SetBool("Walk", true);
            }

            choicetimer = 0;
            if(isBoss)
            {
                nextattack = Random.Range(0, 3);
            }
            else
            {
                nextattack = 0;
            }
            
        }



        if (myPathFinding.atTarget)
        {
            myAnimator.SetBool("Walk", false);
            ChooseNextAttack();
        }
        else
        {
            
           // myAnimator.SetBool("Smash", false);
            //myAnimator.SetBool("Spin", false);
            //myAnimator.SetBool("Attack", false);
            //myAnimator.SetBool("Walk", true);
        }
    }

    public void ChooseNextAttack()
    {
        
        //choosing = false;

        if(nextattack == 0 && myPathFinding.atTarget)
        {
            //myAnimator.ResetTrigger("Revert");
            myAnimator.SetBool("Attack", true);
        }
        else if(nextattack == 1 && myPathFinding.atTarget)
        {
            myAnimator.ResetTrigger("Revert");
            myAnimator.SetBool("Spin", true);
        }
        else if(nextattack == 2 && myPathFinding.atTarget)
        {
            myAnimator.ResetTrigger("Revert");
            myAnimator.SetBool("Spin", true);
        }
        else if(nextattack == 3 && myPathFinding.atTarget)
        {
            myAnimator.ResetTrigger("Revert");
            myAnimator.SetBool("Spin", true);
        }
    }

    public void SpinAttack()
    {

    }
    public void SmashAttack()
    {

    }



    public void LeapSmash()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 3000 + Vector3.up  * 1500 + Vector3.right);
        print("Leap");
    }

    public void LeapSpin()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 3000 + Vector3.up * 6 + Vector3.right *  -100);
        print("Spin");
    }

    public void LeapBack()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 0 + Vector3.up * 2 + Vector3.right, ForceMode.Impulse);
    }

    void Attack()
    {
        
        myAnimator.SetTrigger("Attack");
        actionCooldown = 1;
        myAnimator.ResetTrigger("Revert");     
    }

    public void ChangeHealth(float amount)
    {
        hurtSound.Play();
        Health += amount;
        if(textHP != null)
        {
            textHP.text = Health.ToString();
        }
        if (isBoss)
        {
            UIManager.instance.interfaceGame.UpdateBossHealth(Health,maxHealth);
        }
        if(amount < 0)
        {
            
            
           
        }
        if(Health <= 0)
        {
            RagdollBones();
            myRoom.EnemyKilled(this);
            myAnimator.enabled = false;
            myPathFinding.enabled = false;
        }
    }

    void RagdollBones()
    {
        if(LootManager.instance != null)
        {
            LootManager.instance.Loot(gameObject.transform,false);
        }
        
        GameObject g = Instantiate(bonepieces, this.gameObject.transform.position, this.gameObject.transform.rotation);
        hurtSound.Play();
        Destroy(g.gameObject, 3);
        List<Rigidbody> rl = new List<Rigidbody>();
        foreach (Transform t in g.transform)
        {
            if(t != g.transform)
            {
                rl.Add(t.GetComponent<Rigidbody>());
            }
        }

        for (int i = 0; i < rl.Count; i++)
        {
            rl[i].AddExplosionForce(1000, Camera.main.transform.position, 20);
            rl[i].AddForce(transform.up * 400);

        }
        

        Destroy(this.gameObject, 0.01f);
    }



}

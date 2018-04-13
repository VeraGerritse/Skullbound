using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatAi : MonoBehaviour {

    public enum Task { Idle, Follow, Attack, Block, Spin, Smash };
    public Task task;

    


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

    public float actionCooldown;
    public bool dualwielder;
    public bool shielded;
    public bool block;
    public bool isBoss;



    void Start()
    {
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
        
    }



    void Update()
    {
        if(!myPathFinding.atTarget)
        {
            task = Task.Follow;
        }
        
        if(myPathFinding.atTarget)
        {
            int moveselect;
            if (isBoss)
            {               
                moveselect = Random.Range(0, 3);
            }
            else
            {
                moveselect = 0;
            }

            if(moveselect == 0)
            {
                task = Task.Attack;
                myAnimator.SetBool("Attack", true);
            }
            else
            {
                myAnimator.SetBool("Attack", false);
            }

            if(moveselect == 1)
            {
                task = Task.Spin;
                myAnimator.SetBool("Spin", true);
            }
            else
            {
                myAnimator.SetBool("Spin", false);
            }

            if (moveselect == 2)
            {
                task = Task.Smash;
                myAnimator.SetBool("Smash", true);

            }
            else
            {

                myAnimator.SetBool("Smash", false);
            }

        }



        if(shielded)
        {
            if(PlayerActions.staticplayerAttacks)
            {
                myAnimator.SetBool("Block", true);
                task = Task.Block;
            }
            else
            {
                myAnimator.SetBool("Block", false);
            }
        }












        if(task == Task.Attack)
        {
            if(actionCooldown <= 0)
            {
                Attack();
            }
        }
        else if(task == Task.Idle)
        {
            myAnimator.SetBool("Walk", false);
        }
        else if(task == Task.Follow)
        {
            myAnimator.SetBool("Walk", true);
        }
        else if(task == Task.Block)
        {
            if (actionCooldown <= 0)
            {

            }
        }
        else if(task == Task.Smash)
        {
            if (actionCooldown <= 0)
            {

            }
        }
        else if(task == Task.Spin)
        {
            if (actionCooldown <= 0)
            {

            }
        }
    }

    public void LeapSmash()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 2100 + Vector3.up  * 1500 + Vector3.right);
        print("Leap");
    }

    public void LeapSpin()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 2000 + Vector3.up * 6 + Vector3.right *  -100);
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
        textHP.text = Health.ToString();
        if(amount < 0)
        {
            
            myAnimator.SetTrigger("Hurt");
           
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
            LootManager.instance.Loot(gameObject.transform);
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

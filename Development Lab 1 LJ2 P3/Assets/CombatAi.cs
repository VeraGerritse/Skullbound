using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAi : MonoBehaviour {

    public Pathfinding myPathFinding;
    public Animator myAnimator;

    public float actionCooldown;

    void Start()
    {
        myPathFinding = GetComponent<Pathfinding>();
        myAnimator = transform.GetChild(0).GetComponent<Animator>();
    }

    void Update()
    {
        //tests
        if(Input.GetKeyDown("1"))
        {
            myAnimator.SetTrigger("test1");
        }
        if (Input.GetKeyDown("2"))
        {
            myAnimator.SetTrigger("test2");
            Attack();
        }
        if (Input.GetKeyDown("3"))
        {

        }
        if (Input.GetKeyDown("4"))
        {


        }
        if(actionCooldown > 0)
        {
            actionCooldown -= Time.deltaTime;
        }

        if(myPathFinding.atTarget && actionCooldown <= 0)
        {
            Attack();
        }


        myAnimator.SetBool("Walk", !myPathFinding.atTarget);


    }

    void Attack()
    {
        myAnimator.SetTrigger("Attack");
        actionCooldown = 1;
    }

}

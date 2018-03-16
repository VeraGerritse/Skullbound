using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAi : MonoBehaviour {

    public Pathfinding myPathFinding;
    public Animator myAnimator;

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
        }
        if (Input.GetKeyDown("3"))
        {

        }
        if (Input.GetKeyDown("4"))
        {

        }
    }

    void Attack()
    {

    }


}

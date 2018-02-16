using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTester : MonoBehaviour {

    private Animator anim;
    public bool canCombo;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
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
            anim.SetTrigger("Draw");
        }

        if(Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Swing");
        }

        if (Input.GetButtonDown("Fire2"))
        {
            anim.SetTrigger("Block");
        }

    }
}

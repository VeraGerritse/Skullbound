using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTester : MonoBehaviour {

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown("r"))
        {
            anim.SetTrigger("Draw");
        }

        if(Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Swing");
        }
    }
}

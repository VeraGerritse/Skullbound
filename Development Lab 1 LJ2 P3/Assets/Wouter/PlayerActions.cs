using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour {

    private Animator anim;
    public bool canCombo;
    public float speedmodifier;

    public CollisionChecker collisionChecker;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(speedmodifier <= 1)
        {
            anim.SetFloat("TestFloat", speedmodifier);
            speedmodifier += Time.deltaTime * 4;
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
        if(Input.GetKeyDown("f"))
        {
            anim.SetFloat("TestFloat", -0.3f);

        }



    }

    public void Hit(float amount)
    {
        if(collisionChecker.hitObject != null)
        {
            speedmodifier = amount;
            if(collisionChecker.hitObject.GetComponent<Rigidbody>() != null)
            {
                collisionChecker.hitObject.GetComponent<Rigidbody>().AddForce(transform.forward * 100);
            }
        }
    }


}

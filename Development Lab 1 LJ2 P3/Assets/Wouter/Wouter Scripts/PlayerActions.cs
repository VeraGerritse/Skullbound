using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour {

    private Animator anim;
    public bool canCombo;
    public bool alternate;
    public float speedmodifier;

    public CollisionChecker collisionChecker;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(speedmodifier < 1)
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
            anim.SetTrigger("Draw");
        }

        if(Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Swing");
            anim.ResetTrigger("TestTrigger");
        }

        if (Input.GetButtonDown("Fire2"))
        {
            anim.SetTrigger("Block");
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

            if(collisionChecker.hitObject.GetComponent<Enemy>() != null)
            {
                if(collisionChecker.hitObject.GetComponent<Enemy>().isBlocking)
                {
                    anim.SetTrigger("TestTrigger");
                    //anim.ResetTrigger("TestTrigger");
                }
            }
        }
    }
    public void SetSpeed(float amount)
    {
        speedmodifier = amount;
    }


}

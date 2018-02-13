using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMent : MonoBehaviour {
    public float horizontal;
    public float vertical;
    public float vert;
    public float hor;
    public float turnSpeed;
    public float walkSpeed;
    public float currentSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public float jumpHeight;
    public Animator anim;
    public Rigidbody player;
    public float updownRange;
    public float vertRot;
    bool inAir;

    bool standStill;

    void Start () {
        currentSpeed = walkSpeed;
        player = GetComponent<Rigidbody>();
	}


    private void FixedUpdate()
    {
        if (!standStill)
        {
        Movement();
        }
    } 

    public void Movement()
    {
        horizontal = Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime;
        vertical = Input.GetAxis("Mouse Y") * turnSpeed * Time.deltaTime;
        vert = Input.GetAxis("Vertical") * (currentSpeed / 10) * Time.deltaTime; 
        hor = Input.GetAxis("Horizontal") * (currentSpeed / 10)* Time.deltaTime;

        if (Input.GetButton("Vertical") && Input.GetButton("Horizontal"))
        {

            //print()
            vert = vert / Mathf.Sqrt(2);
            //hor = hor / Mathf.Sin(Mathf.PI / 4);
            hor = hor / Mathf.Sqrt(2);
        }

        Vector3 newLoc = new Vector3(hor,0,vert);

        transform.Translate(newLoc);

        transform.Rotate(0, horizontal, 0);
        vertRot -= vertical;
        vertRot = Mathf.Clamp(vertRot, -updownRange, updownRange);
        Camera.main.transform.localRotation = Quaternion.Euler(vertRot, 0, 0);

        if (Input.GetButton("Crouch"))
        {
            Crouch();
        }
        if (Input.GetButtonUp("Crouch"))
        {
            currentSpeed = walkSpeed;
            anim.SetBool("Crouch", false);
        }
        if (Input.GetButton("Run"))
        {
            Run();
        }
        if (Input.GetButtonUp("Run"))
        {
            currentSpeed = walkSpeed;
        }
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    public void Crouch()
    {
        anim.SetBool("Crouch", true);
        currentSpeed = crouchSpeed;
    }

    public void Jump()
    {
        print("test1");
        Vector3 fwd = transform.TransformDirection(-Vector3.up);
        //if(Physics.CapsuleCast(transform.position,new Vector3(transform.position.x,transform.position.y - 10f, transform.position.z), 1, -transform.up, 10f))
        //{
        //    print("test");
        //    player.AddForce(transform.up * jumpHeight);
        //}
        RaycastHit rayHit;
        if (Physics.Raycast(transform.position, fwd, out rayHit, 1.1f))
        {
            if(rayHit.collider.tag == "Ground")
            {
                player.AddForce(transform.up * jumpHeight);
            }
        }
    }

    public void Run()
    {
        currentSpeed = runSpeed;
    }
}

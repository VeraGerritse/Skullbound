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
    public float modifier;
    public float jumpHeight;
    public Animator anim;
    public Rigidbody player;
    public float updownRange;
    public float vertRot;
    
    bool inAir;

    public GameObject cameraObject;

    bool standStill = false;

    void Start () {
        currentSpeed = walkSpeed;
        player = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void FixedUpdate()
    {
        if (!standStill)
        {
            Movement();
        }
    }

    private void Update()
    {
        
        // "ButtonUp" & "ButtonDown" cant be in FixedUpdate or they have a chance to be skipped.

        // Don't double dip with fixed update & deltatime.

        Looking();

        if (Input.GetButton("Crouch"))
        {
            Crouch();
        }
        if (Input.GetButtonUp("Crouch"))
        {
            currentSpeed = walkSpeed;
            anim.SetBool("Crouch", false);
            anim.ResetTrigger("Block");
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

    public void Looking()
    {

    }

    public void Movement()
    {
        horizontal = Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime;
        vertical = Input.GetAxis("Mouse Y") * turnSpeed * Time.deltaTime;

        vertRot -= vertical;
        vertRot = Mathf.Clamp(vertRot, -updownRange, updownRange);
        cameraObject.transform.localRotation = Quaternion.Euler(vertRot, 0, 0);

        vert = Input.GetAxis("Vertical") * (currentSpeed * modifier / 10) * Time.deltaTime; 
        hor = Input.GetAxis("Horizontal") * (currentSpeed * modifier / 10)* Time.deltaTime;

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



    }

    public void Crouch()
    {
        anim.SetBool("Crouch", true);
        anim.SetTrigger("UnBlock");
        
        currentSpeed = crouchSpeed;
    }

    public void Jump()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hit, 1.1f))
        {
            if (hit.transform.gameObject != null)
            {
                //print("Player jumped from "  + hit.transform.gameObject.name);
                player.AddForce(transform.up * jumpHeight);
            }
        }
    }

    public void Run()
    {
        currentSpeed = runSpeed;
    }
}

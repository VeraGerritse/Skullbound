using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inertia : MonoBehaviour {
    private Animator anim;

    public int inverse;

    public bool canSway;

    public float twitchidlestrenght;
    //public bool canTwitch;
    

    public float amount;
    public float maxAmount;
    public float smoothAmount;

    public float bobbingStrenght;
    public float bobbyModifier;

    public Rigidbody playerbody;
    public GameObject viewmodelcamera;
    public float yVelocity;

    //public Vector3 offset;
    public Vector3 initialPosition;
    

    private void Start()
    {
        initialPosition = transform.localPosition;
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        playerbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        viewmodelcamera = GameObject.FindGameObjectWithTag("ViewModelCamera");


    }

    public void IHateMyLife(bool depression)
    {
        //canTwitch = depression;
    }


    void FixedUpdate()
    {

    }

    private void Update()
    {

        

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            bobbingStrenght += Time.deltaTime * 2;
        }
        else
        {
            bobbingStrenght -= Time.deltaTime * 4;
        }

        bobbingStrenght = Mathf.Clamp(bobbingStrenght, 0, bobbyModifier);
        anim.SetLayerWeight(4, bobbingStrenght);




        //print(anim.GetLayerWeight(4));

        //print(canTwitch);
        //anim.SetBool("CanTwitch", canTwitch);
        //if(anim.GetCurrentAnimatorClipInfo)
        //canTwitch = true;


        //anim.SetLayerWeight(4, twitchidlestrenght);
        //anim.SetLayerWeight(1, twitchidlestrenght);



        yVelocity = playerbody.velocity.y;

        float movementX = -Input.GetAxis("Mouse X") * amount + -Input.GetAxis("Horizontal") * amount;
        float movementY = -Input.GetAxis("Mouse Y") * amount ;
        float movementZ = -Input.GetAxis("Vertical") * amount * 0.5f;

        if (!canSway)
        {
            movementX = 0;
            movementY = 0;
            movementZ = 0;
            yVelocity = 0;
        }

        movementX = Mathf.Clamp(movementX, -maxAmount, maxAmount);
        movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount);
        movementZ = Mathf.Clamp(movementZ, -maxAmount, maxAmount * 0.5f);

        Vector3 finalPosition = new Vector3(inverse * movementX, movementY + -yVelocity *0.025f, movementZ);
        
        transform.localPosition = Vector3.Slerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmount);
        //viewmodelcamera.transform.localPosition = Vector3.Slerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmount);




        if (Input.GetKeyDown("k"))
        {
            //canSway = !canSway;
        }

        if (Input.GetButton("Crouch"))
        {
            canSway = false;
        }
        else
        {
            canSway = true;
        }




    }
}

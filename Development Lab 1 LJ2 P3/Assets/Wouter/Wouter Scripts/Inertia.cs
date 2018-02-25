using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inertia : MonoBehaviour {

    public bool canSway;

    public float amount;
    public float maxAmount;
    public float smoothAmount;

    public Rigidbody playerbody;
    public float yVelocity;

    //public Vector3 offset;
    private Vector3 initialPosition;
    

    private void Awake()
    {
        initialPosition = transform.localPosition;
        
    }

    private void Update()
    {
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
        movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount *2);
        movementZ = Mathf.Clamp(movementZ, -maxAmount, maxAmount * 0.5f);

        Vector3 finalPosition = new Vector3(movementX, movementY + -yVelocity *0.025f, movementZ);
        
        transform.localPosition = Vector3.Slerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmount);




        if (Input.GetKeyDown("k"))
        {
            canSway = !canSway;
        }




    }
}

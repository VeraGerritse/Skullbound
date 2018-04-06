using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Interactables
{

    public bool canBePickedUp;

    public Renderer rend;

    public bool doOutLine;

    public void Start()
    {
        rend = this.GetComponent<Renderer>();
    }

    public override void Interact()
    {
            canBePickedUp = true;
    }

    public override void StopInteract()
    {
        canBePickedUp = false;
    }



}

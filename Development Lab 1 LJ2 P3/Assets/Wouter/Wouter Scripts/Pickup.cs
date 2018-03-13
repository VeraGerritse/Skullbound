using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Interactables
{

    public bool canBePickedUp;

    public override void Interact()
    {
            canBePickedUp = true;
    }

    public override void StopInteract()
    {
        canBePickedUp = false;
    }

}

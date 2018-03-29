using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactables {
    public Animator anim;

    public void CloseDoors()
    {
        anim.SetBool("Locked", true);
    }

    public void OpenDoor()
    {
        anim.SetBool("Locked", false);
    }
}

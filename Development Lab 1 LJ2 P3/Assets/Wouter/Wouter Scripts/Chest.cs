using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactables {

    public Animator anim;


    public void Open()
    {

    }

    public override void OpenChest()
    {
        if (!used)
        {
            used = true;
            anim.SetTrigger("Open");
            StartCoroutine(Loot());
        }
    }

    IEnumerator Loot()
    {
        yield return new WaitForSeconds(2f);
        LootManager.instance.Loot(transform, true);
    }
}

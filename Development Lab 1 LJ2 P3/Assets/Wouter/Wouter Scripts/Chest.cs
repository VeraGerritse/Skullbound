using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactables {

    public Animator anim;
    public ParticleSystem PS;
    public GameObject myLoot;

    public override void Interact()
    {
        if(myLoot != null)
        {
            print("werkt dit?");
            Vector3 endLoc = new Vector3(myLoot.transform.position.x, myLoot.transform.position.y + 1f, myLoot.transform.position.z);
            myLoot.transform.position = Vector3.Lerp(myLoot.transform.position, endLoc, 1f * Time.deltaTime);
            //myLoot.transform.position = newPos;
            StartCoroutine(GoHigher());
        }
    }

    public override void OpenChest()
    {
        if (!used)
        {
            used = true;
            anim.SetTrigger("Open");
            PS.Play();
            StartCoroutine(Loot());
        }
    }

    IEnumerator Loot()
    {
        yield return new WaitForSeconds(1f);
        print(myLoot);
        myLoot = LootManager.instance.Loot(transform, true);
        print(myLoot);
    }

    IEnumerator GoHigher()
    {
        yield return new WaitForSeconds(1f);
        myLoot = null;
        yield return new WaitForSeconds(3f);
        PS.Stop();
    }
}

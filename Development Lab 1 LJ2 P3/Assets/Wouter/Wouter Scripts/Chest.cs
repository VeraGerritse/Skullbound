using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactables {

    public Animator anim;
    public ParticleSystem PS;
    public GameObject myLoot;
    public bool rising;
    public bool bossChest;

    public override void Interact()
    {
        print(myLoot);
        if(myLoot != null && !rising)
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
        print("whut?");
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
        myLoot = LootManager.instance.Loot(transform, true, bossChest);
        myLoot.GetComponent<Collider>().enabled = false;
        print(myLoot);
    }

    IEnumerator GoHigher()
    {
        yield return new WaitForSeconds(1f);
        rising = true;
        myLoot.GetComponent<Collider>().enabled = true;
        yield return new WaitForSeconds(3f);
        PS.Stop();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" && myLoot != null)
        {
            myLoot.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}

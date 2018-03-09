using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public GameObject owner;
    public bool followPlayer;
    private float timer;
    public Vector3 weaponslot;

    //stats
    public string weaponName;
    public int itemId;
    public float attack;
    public float defence;
    public float staminacost;
  
    //enemy only

    void Start()
    {
        timer = 0;
        this.gameObject.SetActive(true);
        weaponslot = GameObject.Find("WeaponSlot").transform.position;
    }
    
    public void OnTriggerEnter(Collider other)       
    {
        if(owner != null)
        {
            if (owner.GetComponent<Enemy>() != null)
            {
                if (other.tag == "Player")
                {
                    if (!other.GetComponent<PlayerStats>().playerBLocks)
                    {
                        other.GetComponent<PlayerStats>().ChangeHealth(-attack + owner.GetComponent<Enemy>().extraAttack);
                        other.GetComponent<PlayerActions>().anim.SetTrigger("Recoil");
                    }
                    else
                    {
                        if (owner != null)
                        {
                            owner.GetComponent<Animator>().SetTrigger("Revert");
                            other.GetComponent<PlayerActions>().anim.SetTrigger("RecoilBlock");
                        }
                    }
                }
            }
        }        
    }
    void Update()
    {
        if(followPlayer)
        {
            FollowPlayer();
            if (GetComponent<Rigidbody>() != null)
            {
                GetComponent<Rigidbody>().useGravity = false;
            }
                
        }
        else
        {
            timer = 0;
            this.gameObject.SetActive(true);
            if(GetComponent<Rigidbody>() != null)
            {
                GetComponent<Rigidbody>().useGravity = true;
            }
            
        }
        
    }
    public void FollowPlayer()
    {
        this.gameObject.GetComponent<MeshCollider>().enabled = false;
        this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position, Time.deltaTime * 14f);
        this.gameObject.transform.rotation = Quaternion.Lerp(this.gameObject.transform.rotation, GameObject.FindGameObjectWithTag("Player").transform.rotation, Time.deltaTime * 10);
        timer += Time.deltaTime;
        if(timer >= 0.25f)
        {
            this.gameObject.SetActive(false);               
        }        
    }
}

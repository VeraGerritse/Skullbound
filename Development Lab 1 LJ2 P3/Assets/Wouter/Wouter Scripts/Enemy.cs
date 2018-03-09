using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public bool isBlocking;

    public Animator anim;

    //stats
    public float health;
    public float stamina;
    public float movementspeed;
    public float attackingspeed;
    public float extraAttack;
    public float extraDefence;

    //gear
    public GameObject weapon;
    public GameObject shield;

    public void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
    }


}

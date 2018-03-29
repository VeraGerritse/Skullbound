using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatAi : MonoBehaviour {

    public Pathfinding myPathFinding;
    public Animator myAnimator;
    public Text textHP;
    public List<GameObject> bones = new List<GameObject>();
    public GameObject bonepieces;
    public RoomActivities myRoom;
    [Header("Stats")]
    public float Health;

    public float actionCooldown;

    void Start()
    {
        textHP.text = Health.ToString();
        myPathFinding = GetComponent<Pathfinding>();
        myAnimator = transform.GetChild(0).GetComponent<Animator>();
    }



    void Update()
    {
        if(Input.GetKeyDown("1"))
        {
            ChangeHealth(-10);
        }

        if(actionCooldown > 0)
        {
            actionCooldown -= Time.deltaTime;
        }

        if(myPathFinding.atTarget && actionCooldown <= 0)
        {
            Attack();
        }
        myAnimator.SetBool("Walk", !myPathFinding.atTarget);
    }

    void Attack()
    {
        myAnimator.SetTrigger("Attack");
        actionCooldown = 1;
        myAnimator.ResetTrigger("Revert");
    }

    public void ChangeHealth(float amount)
    {
        Health += amount;
        textHP.text = Health.ToString();
        if(amount < 0)
        {
            myAnimator.SetTrigger("Hurt");
            actionCooldown = 0.5f;
        }
        if(Health <= 0)
        {
            RagdollBones();
            myRoom.EnemyKilled(this);
            myAnimator.enabled = false;
            myPathFinding.enabled = false;
        }
    }

    void RagdollBones()
    {
        GameObject g = Instantiate(bonepieces, this.gameObject.transform.position, this.gameObject.transform.rotation);
        Destroy(g.gameObject, 3);
        List<Rigidbody> rl = new List<Rigidbody>();
        foreach (Transform t in g.transform)
        {
            if(t != g.transform)
            {
                rl.Add(t.GetComponent<Rigidbody>());
            }
        }

        for (int i = 0; i < rl.Count; i++)
        {
            rl[i].AddExplosionForce(1000, Camera.main.transform.position, 20);
            rl[i].AddForce(transform.up * 400);

        }
        

        Destroy(this.gameObject, 0.01f);
    }

}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomActivities : MonoBehaviour {

    //public List<GameObject> rBodys = new List<GameObject>();
    public List<Interactables> interactable = new List<Interactables>();
    public List<GameObject> enemys = new List<GameObject>();
    public List<Transform> spawnLoc = new List<Transform>();
    public List<CombatAi> enemysAlive = new List<CombatAi>();
    public bool bossRoom;
    bool roomDone;
    public RoomGen myRoom;
    public int SpawnRate;
    bool inRoom;
    bool cleared;
    public bool roomForTesting;

    private void Start()
    {
        myRoom = GetComponentInParent<RoomGen>();
        myRoom.myActivities = this;
    }


    public void EnableRigidBodys()
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void DisableRigidBodys()
    {
        foreach(Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void StartInteracting()
    {
        if(spawnLoc.Count != 0 && enemys.Count != 0)
        {
            StartCoroutine(SpawnSkeletons());          
        }
        EnemyKilled(null);
        for (int i = 0; i < interactable.Count; i++)
        {
            interactable[i].IsAwake = true;
        }
    }

    public void StopInteracting()
    {
        for (int i = 0; i < interactable.Count; i++)
        {
            interactable[i].IsAwake = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && inRoom)
        {
            inRoom = false;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player" && !inRoom && DungeonGeneratorManager.instance.ready && myRoom !=null)
        {

            ClearManager.instance.EnterRoom();
            StartInteracting();
            inRoom = true;
            StartCoroutine(DungeonGeneratorManager.instance.EnterRoom(myRoom));
            
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRoom = false;
            StopInteracting();
        }
    }

    public IEnumerator SpawnSkeletons()
    {
        yield return new WaitForSeconds(0.01f);
        int spawnChance = 0;
        for (int i = 0; i < spawnLoc.Count; i++)
        {
            yield return new WaitForSeconds(0.1f);
            spawnChance = Random.Range(0, 100);
            if (bossRoom)
            {
                spawnChance = 0;
            }
            if (spawnChance < SpawnRate && !cleared && roomForTesting)
            {
                GameObject newEnemy = Instantiate(enemys[Random.Range(0, enemys.Count)], spawnLoc[i].position, Quaternion.identity);
                newEnemy.GetComponent<CombatAi>().myRoom = this;
                newEnemy.GetComponent<Pathfinding>().IsAwake = true;
                enemysAlive.Add(newEnemy.GetComponent<CombatAi>());
            }
            else if (spawnChance < SpawnRate && !cleared)
            {
                GameObject newEnemy = Instantiate(TierManager.instance.RandomSkelleton(bossRoom), spawnLoc[i].position, Quaternion.identity);
                newEnemy.GetComponent<CombatAi>().myRoom = this;
                newEnemy.GetComponent<Pathfinding>().IsAwake = true;
                enemysAlive.Add(newEnemy.GetComponent<CombatAi>());
            }
        }
        cleared = true;
    }

    public void EnemyKilled(CombatAi deadEnemy)
    {
        if (enemysAlive.Count == 0)
        {
            if (bossRoom)
            {
                TierManager.instance.HigherTier();
                return;
            }
            ClearManager.instance.ExitRoom();
            return;
        }
        for (int i = 0; i < enemysAlive.Count; i++)
        {
            if (enemysAlive[i] == deadEnemy)
            {
                enemysAlive.RemoveAt(i);
                if (enemysAlive.Count == 0)
                {
                    if (bossRoom)
                    {
                        TierManager.instance.HigherTier();
                        return;
                    }
                    ClearManager.instance.ExitRoom();
                    return;
                }
            }
        }

    }
}

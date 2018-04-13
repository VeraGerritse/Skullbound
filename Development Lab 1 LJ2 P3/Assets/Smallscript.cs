using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smallscript : MonoBehaviour {

    public CombatAi combatai;
    public bool thisbool;
    public SpawnDust spawnduster;

    private void FixedUpdate()
    {
        combatai.block = thisbool;
    }

    public void LeapSmash()
    {
        combatai.LeapSmash();
    }
    public void LeapSpin()
    {
        combatai.LeapSpin();
    }
    public void LeapBack()
    {
        combatai.LeapBack();
    }

    public void SpawnDustNOWDAMNIT()
    {
        spawnduster.SpawnADust();
    }

    public void CauseShake()
    {
        GameObject.FindWithTag("Player").GetComponent<Animator>().SetLayerWeight(5, 1 / Vector3.Distance(this.gameObject.transform.position, Camera.main.transform.position));
        GameObject.FindWithTag("Player").GetComponent<Animator>().SetTrigger("Shake");
    }

}

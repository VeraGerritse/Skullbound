using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smallscript : MonoBehaviour {

    public CombatAi combatai;
    public bool thisbool;

    private void FixedUpdate()
    {
        combatai.block = thisbool;
    }

}

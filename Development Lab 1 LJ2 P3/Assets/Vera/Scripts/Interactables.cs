using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour {

    [Header("awake and if it stays awake or just once")]
    public bool IsAwake;
    public bool keepsWorking;
    bool workedOnce;

    public void Update()
    {
        if (keepsWorking && IsAwake)
        {
            Interact();
        }
        if(!keepsWorking && IsAwake && !workedOnce)
        {
            Interact();
            workedOnce = true;
        }
    }

    public virtual void Interact()
    {

    }
}

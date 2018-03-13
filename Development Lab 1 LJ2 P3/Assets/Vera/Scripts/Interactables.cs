﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour {

    [Header("awake and if it stays awake or just once")]
    public bool IsAwake;
    public bool keepsWorking;
    bool workedOnce;
    bool onOrOff;

    public void Update()
    {
        if (keepsWorking && IsAwake)
        {
            Interact();
            onOrOff = true;
        }
        if(!keepsWorking && IsAwake && !workedOnce)
        {
            Interact();
            workedOnce = true;
            onOrOff = true;
        }
        if(!IsAwake && !onOrOff)
        {
            onOrOff = false;
            StopInteract();
        }
    }

    public virtual void Interact()
    {

    }

    public virtual void StopInteract()
    {

    }
}

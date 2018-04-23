﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour {

    [Header("awake and if it stays awake or just once")]
    public bool IsAwake;
    public bool keepsWorking;
    public bool used;
    bool workedOnce;
    bool onOrOff;
    public bool chest;

    public float myFloat;

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
        if (chest)
        {
            Interact();
        }

        myFloat -= Time.deltaTime;

        myFloat = Mathf.Clamp(myFloat, 0, 0.5f);

        if (myFloat >= 0.5f)
        {

        }
    }
    public virtual void OpenChest()
    {
        workedOnce = false;
    }
    public virtual void Interact()
    {

    }

    public virtual void StopInteract()
    {

    }
}

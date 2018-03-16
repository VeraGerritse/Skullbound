using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour {
    public PlayerActions actions;
    public static InteractManager instance;
    public LayerMask layers;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Update()
    {
        if (actions != null)
        {
            print("1");
            if (actions.anim != null)
            {
                print("2");
                if (actions.anim.GetCurrentAnimatorStateInfo(2).IsTag("lol"))
                {
                    print("3");
                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * 3, out hit, 3, layers))
                    {
                        print("test");
                        if (hit.collider.gameObject != null)
                        {
                            print(hit.collider.gameObject.tag);
                            if (hit.collider.gameObject.tag == "Interactable" || hit.collider.gameObject.tag == "FrontDoor" || hit.collider.gameObject.tag == "BackDoor")
                            {
                                UIManager.instance.interfaceGame.inter = true;
                            }
                            else
                            {
                                UIManager.instance.interfaceGame.inter = false;
                            }
                        }
                        else
                        {
                            UIManager.instance.interfaceGame.inter = false;
                        }
                    }
                    else
                    {
                        UIManager.instance.interfaceGame.inter = false;
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour {
    public PlayerActions actions;
    public static InteractManager instance;
    public LayerMask layers;
    public GameObject raycastobj;

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
            if (actions.anim != null)
            {
                if (actions.anim.GetCurrentAnimatorStateInfo(2).IsTag("lol"))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * 3, out hit, 3, layers))
                    {
                        if (hit.collider.gameObject != null)
                        {
                            if (hit.collider.gameObject.GetComponentInParent<Interactables>() != null)
                            {
                                
                                if (hit.collider.gameObject.tag == "Interactable")
                                {
                                    if(!hit.collider.gameObject.GetComponentInParent<Interactables>().used)

                                    UIManager.instance.interfaceGame.inter = true;
                                    if (hit.transform.GetComponent<Pickup>() != null)
                                    {

                                        hit.transform.gameObject.GetComponent<Pickup>().myFloat += Time.deltaTime * 2;
                                    }
                                }
                                else
                                {

                                    UIManager.instance.interfaceGame.inter = false;
                                }
                            }
                            else if(hit.collider.gameObject.GetComponent<Interactables>() != null)
                            {
                                if (!hit.collider.gameObject.GetComponent<Interactables>().used)
                                {
                                    if (hit.collider.gameObject.tag == "Interactable")
                                    {
                                        UIManager.instance.interfaceGame.inter = true;
                                        if (hit.transform.GetComponent<Pickup>() != null)
                                        {

                                            hit.transform.gameObject.GetComponent<Pickup>().myFloat += Time.deltaTime * 2;
                                        }
                                    }
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
                    else
                    {

                        UIManager.instance.interfaceGame.inter = false;
                    }
                }
            }
        }
    }
}

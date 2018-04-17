using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovement : MonoBehaviour
{
    public Transform player;

    public int hight;
    public bool cam;

    private void Awake()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }
    void Update ()
    {   if(player != null)
        {
            Vector3 playerLoc = new Vector3(player.position.x, player.position.y + hight, player.position.z);
            transform.position = playerLoc;
        }
	}
}

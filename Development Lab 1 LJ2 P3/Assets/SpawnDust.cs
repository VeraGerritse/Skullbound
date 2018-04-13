using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDust : MonoBehaviour
{

    public GameObject dust;


    public void SpawnADust()
    {
        Instantiate(dust, this.gameObject.transform.position, transform.rotation);
    }
}

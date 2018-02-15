using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public static MapManager instance;

    public List<GameObject> allChambers = new List<GameObject>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void ListLenght(int length)
    {
        allChambers.Clear();
        for (int i = 0; i < length; i++)
        {
            allChambers.Add(null);
        }
    }
}

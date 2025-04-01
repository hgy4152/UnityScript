using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;

    public GameObject[] DungeonList;

    public int DungeonIndex;

    void Awake()
    {
        instance = this;
        DungeonIndex = DungeonList.Length - 1;
    }

}

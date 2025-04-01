using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Info")]
    public float speed;
    public float asd;
    public float msd;
    public float damage;
    public float maxHealth;
    public float maxAmmor;
    public float crit;
    public float critDamage;

    [Header("Dungeon Control")]
    public int[] monIndex;
    public int monCount;
    public int DgLv;



    void Awake()
    {
        instance = this;
    }
}

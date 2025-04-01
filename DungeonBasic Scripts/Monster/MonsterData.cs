using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Monster", menuName = "Scriptble Object/MonsterData")]
public class MonsterData : ScriptableObject
{
    public enum MonsterType { zero, one, two, three, four, five };

    [Header("Status")]
    public MonsterType monType;
    public int monId;
    public string monName;
    public Sprite monImg;
    public float monHealth;
    public float monAmmor;
    public float monPower;
    public float monMsd;
    public float monAsd;

}

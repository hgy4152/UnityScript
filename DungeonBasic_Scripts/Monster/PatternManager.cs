using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatternManager : MonoBehaviour
{
    public abstract void Battle(Animator anim, MonsterData data);
}

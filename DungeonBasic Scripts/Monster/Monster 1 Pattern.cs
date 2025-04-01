using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Monster1Pattern : PatternManager
{
    public Animator animator;
    public MonsterData monData;

    public GameObject obj;

    bool isAttack = false;
    bool Hurt;

    BoxCollider2D attackArea;

    void Start()
    {
        attackArea = obj.GetComponent<BoxCollider2D>();

    }


    public override void Battle(Animator anim, MonsterData data)
    {
        animator = anim;
        monData = data;


        Hurt = animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Attack") && !Hurt && collision.GetComponent<DamageManager>().atkDamage > 1)
        {
            animator.SetTrigger("doHurt");
        }
    }

    void OnTriggerStay2D(Collider2D area)
    {

        if (area.CompareTag("HitBox") && !isAttack && !Hurt)
        {
            //animator.SetBool("isAttack", true);
            StartCoroutine(Melee());
        }

    }
    IEnumerator Melee()
    {
        isAttack = true;
        animator.SetTrigger("doAttack");

        animator.speed = monData.monAsd;


        yield return new WaitForSeconds((float)1f / monData.monAsd);

        isAttack = false;

    }

    void OnCollisionExit2D(Collision2D collision)
    {
        StopCoroutine(Melee());
        isAttack = false;

    }

    void Rush()
    {
        Debug.Log("Rush");
    }

    void Spell()
    {
        Debug.Log("Spell");
    }
}

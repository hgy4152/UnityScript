using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static MonsterData;
using static UnityEngine.Rendering.VolumeComponent;


public class Enemy : MonoBehaviour
{
    public Rigidbody2D target;
    public RuntimeAnimatorController[] animCon;
    public MonsterData[] monData;
    public PatternManager[] pattern;
    public PatternManager currpattern;

    [Header("Status")]
    public MonsterType mType;
    public int mId;
    public string mName;
    public Sprite mImg;
    public float mHealth;
    public float mAmmor;
    public float mPower;
    public float mMsd;
    public float mAsd;


    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    MonsterData data;

    bool isLive = true;



    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // �� ������ �ٿ� : ���߿� ��ȸ�Ǹ� ����ȭ ��Ű�� �͵� �����غ���
        Monster();

    }


    void FixedUpdate()
    {


        if (isLive)
        {
            // ���
            if (mHealth <= 0)
            {
                isLive = false;
                StartCoroutine(Death());
            }

            // ���� �ִϸ��̼� �̸� �����ϰ� ���... ���� �� �̵� �Ұ�
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                return;
            }


            anim.SetFloat("Speed", data.monMsd);
            Vector2 dirVec = target.position - rigid.position;
            Vector2 nextVec = dirVec.normalized * data.monMsd * Time.fixedDeltaTime; // �ӵ� ����

            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero; // ���� �������� �߻��ϴ� �߰� �ӵ� ���� : �˹� ��

            // ����
            currpattern.Battle(anim, data);
        }

        }
    void LateUpdate()
    {

        if (mId == 1) { 
            // ���ؿ� �̹��� �ΰ��� �ݴ�����̶� ��¿ �� ���� ���� �ڵ�
            spriter.flipX = target.position.x < rigid.position.x;
            return;
        }


        spriter.flipX = target.position.x > rigid.position.x;

    }

    void Monster()
    {


        // ���� ���� �ڵ� : �������� �ϴ���, ���ӸŴ������� �ε��� �޾ƿ�����
        //int index = GameManager.instance.monIndex[Random.Range(0,GameManager.instance.monIndex.Length)];
        int index = 0;

        transform.GetChild(index).gameObject.SetActive(true);

        data = monData[index];


        mType = data.monType;
        anim.runtimeAnimatorController = animCon[data.monId];
        mId = data.monId;
        mName = data.name;
        mImg = data.monImg;
        mHealth = data.monHealth;
        mAmmor = data.monAmmor;
        mPower = data.monPower;
        mMsd = data.monMsd;
        mAsd = data.monAsd;

        currpattern = pattern[index];

    }

    IEnumerator Death()
    {
 
        anim.SetTrigger("doDeath");

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);    
    }

}

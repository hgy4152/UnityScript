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

        // 몹 데이터 다운 : 나중에 기회되면 직렬화 시키는 것도 생각해보기
        Monster();

    }


    void FixedUpdate()
    {


        if (isLive)
        {
            // 사망
            if (mHealth <= 0)
            {
                isLive = false;
                StartCoroutine(Death());
            }

            // 공격 애니메이션 이름 통일하고 사용... 공격 중 이동 불가
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                return;
            }


            anim.SetFloat("Speed", data.monMsd);
            Vector2 dirVec = target.position - rigid.position;
            Vector2 nextVec = dirVec.normalized * data.monMsd * Time.fixedDeltaTime; // 속도 조절

            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero; // 물리 현상으로 발생하는 추가 속도 제거 : 넉백 등

            // 전투
            currpattern.Battle(anim, data);
        }

        }
    void LateUpdate()
    {

        if (mId == 1) { 
            // 구해온 이미지 두개가 반대방향이라 어쩔 수 없이 넣은 코드
            spriter.flipX = target.position.x < rigid.position.x;
            return;
        }


        spriter.flipX = target.position.x > rigid.position.x;

    }

    void Monster()
    {


        // 몬스터 선택 코드 : 랜덤으로 하던가, 게임매니저에서 인덱스 받아오던가
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

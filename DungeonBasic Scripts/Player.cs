using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed = 3; // �÷��̾� �ӵ�
    public float JumpForce = 200;
    public GameObject attackArea;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriter;
    public CapsuleCollider2D HitBox;
    public DamageManager dmgManager;

    public bool isBorder;
    float totalDamage = 0;

    List<float> DamageBuff;
    List<float> CritBuff;
    List<float> CritDamageBuff;
    List<float> TotalDamageBuff;


    [Header("Player Action")]
    bool isJump;
    bool isAttack;
    bool isDash;
    bool isRun;

    bool isCrit = false;


    void Awake()
    {

        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();

        List<float> DamageBuff = new List<float>();
        List<float> CritBuff = new List<float>();
        List<float> CritDamageBuff = new List<float>();
        List<float> TotalDamageBuff = new List<float>();
    }

    void Update()
    {

        speed = GameManager.instance.speed * (1 + (GameManager.instance.msd / 100));

        TotalDamage();

    }


    void FixedUpdate()
    {
        StopWall();


        if (Input.GetButtonDown("Dash") && !isDash && inputVec != Vector2.zero)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetButtonDown("Jump") && !isJump)
        {
            StartCoroutine(Jump());

        }


        // �÷��̾� �̵�
        Vector2 nexVec = inputVec.normalized * speed * Time.fixedDeltaTime;


        // ���� �߿��� �̵�. �̷��� �ؾ� Ű�Է� ���� ���� �����̰Բ� ���� ����
        if (isJump)
        {
            transform.position += new Vector3(nexVec.x, nexVec.y);
            
        }


        if (!isBorder && !isJump && !isAttack) { 
            rigid.MovePosition(rigid.position + nexVec);

        }


    }


    void LateUpdate()
    {

        if(!isJump)
            anim.SetFloat("Speed", inputVec.magnitude);

        // �¿� ����
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }



    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    void StopWall()
    {
        Debug.DrawRay(transform.position, inputVec * 1.2f, Color.red);
        isBorder = Physics2D.Raycast(transform.position, inputVec, 1, LayerMask.GetMask("Wall"));
    }

    void OnFire()
    {
        if (!isAttack)
        {


            StartCoroutine(Attack());

        }
    }

    IEnumerator Dash()
    {
        StopCoroutine(Attack());
        isAttack = false;
        attackArea.SetActive(false);


        float msd = 1 + GameManager.instance.msd/100;


        anim.speed *= msd;
        GameManager.instance.speed *= 2;

        isDash = true;
        Debug.Log("Dash");
        anim.SetTrigger("doDash");

        // �̵��Ÿ� ����
        yield return new WaitForSeconds(0.3f /msd);

       
        GameManager.instance.speed /= 2;
        anim.speed /= msd;

        // �lŸ��
        yield return new WaitForSeconds(1f);

        isDash = false;
    }

    IEnumerator Jump()
    {
        isJump = true;
        Debug.Log("Jump");
        anim.SetTrigger("doJump");
        
        
        // ������ ���� �÷�ġ�� �� ����. �⺻���� JumpForce = mass x linear drag
        // JumpForce �� �����̻����� �����ϰ� �Ϸ��� ����

        rigid.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.25f);

        // drag ����ؼ� �߷� ����
        rigid.gravityScale = 80/9.81f;
        anim.SetBool("isJump", true);

        yield return new WaitForSeconds(0.25f);

        rigid.gravityScale = 0;


        anim.SetBool("isJump", false);
        isJump = false;
    }

    IEnumerator Attack()
    {
        isAttack = true;

        if (isDash)
        {
            anim.SetTrigger("doDAttack");
            isDash = false;
        }
        else
        {
            anim.SetTrigger("doAttack");
        }



        // ���� �ӵ� ����
        anim.speed = GameManager.instance.asd;


        yield return new WaitForSeconds((float)0.5f/GameManager.instance.asd);

        // 1Ÿ
        attackArea.SetActive(true);
        dmgManager.Atk(totalDamage);

        yield return new WaitForSeconds((float)0.5f /GameManager.instance.asd);

        // 2Ÿ(ĵ�� ����)

        isAttack = false;
        attackArea.SetActive(false);
        anim.speed = 1;
    }



    void TotalDamage()
    {

        float damage = GameManager.instance.damage;
        float crit = GameManager.instance.crit;
        float critDamage = GameManager.instance.critDamage;


        // �⺻����
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            // ������ ���� ���� : ���� ���� �޴� ��ŭ �����ֱ�
            float atkdamage = damage * 1f;

            if (DamageBuff != null) {
                for (int i = 0; i < DamageBuff.Count; i++)
                {
                
                        atkdamage *= DamageBuff[i];
                }
            }
            // ũ��Ƽ�� : ���� �־��ٰŸ� ���� ������ ó�� ���� �Ѱ� ���� �������
            isCrit = Random.Range(0, 101) <= crit ? true : false;
            critDamage = isCrit ? critDamage : 1f;


            // �� ������
            totalDamage = damage * critDamage;

        }

    }

}

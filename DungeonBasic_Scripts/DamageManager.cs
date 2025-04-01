using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{

    bool isDash;
    public float atkDamage;

    float enemyHealth;
    float enemyAmmor;

    // 개인 버프, 디버프를 고려해서 데미지 합산

    public void Atk(float damage)
    {
        atkDamage = damage;

        Debug.Log(atkDamage);
    }

    public void Spell()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            enemyAmmor = collision.GetComponent<Enemy>().mAmmor;
            enemyHealth = collision.GetComponent<Enemy>().mHealth;


            if(atkDamage > enemyAmmor) 
                enemyHealth = enemyHealth - (atkDamage - enemyAmmor);

            Debug.Log(enemyHealth);
            collision.GetComponent<Enemy>().mHealth = enemyHealth;
        }
    }
}

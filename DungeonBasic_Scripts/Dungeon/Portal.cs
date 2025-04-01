using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    public int index;

    // Start is called before the first frame update
    void Start()
    {
 
        index = 0;

    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D area)
    {
        if (area.CompareTag("Player"))
            StartCoroutine(PortalWarp());

    }


    IEnumerator PortalWarp()
    {
        
        yield return new WaitForSeconds(2f);

        
   
        Debug.Log("이동 중...");
        // 지금방 Off
        DungeonManager.instance.DungeonList[0].SetActive(false);        // 이동 메커니즘에 따라 index 값 변화
        index++;

        // 다음 방 On
        DungeonManager.instance.DungeonList[index].SetActive(true);
        

    }

    void OnTriggerExit2D(Collider2D area)
    {
        if(area.CompareTag("Player"))
        {
            StopCoroutine(PortalWarp());
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System;

public class Follow : MonoBehaviour
{
    public Rigidbody2D target;

    void FixedUpdate()
    {
        // 두 물체의 벡터를 이용해서 각도 구하기 >> 공격인식 입체화

        Vector3 vec1 = target.position;
        Vector3 vec2 = transform.parent.position;
        Vector3 vec3 = transform.position;



        float ang = Vector2.Angle(vec1, vec3);
        transform.rotation = Quaternion.Euler(0, 0, ang);// .Euler : 도로 값 받기. 안쓰면 라디안값
        

        transform.position = vec2 + (vec1 - vec2).normalized * 0.76f; // 범위 좌표 곱해줌
    }
}

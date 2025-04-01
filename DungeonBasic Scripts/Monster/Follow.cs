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
        // �� ��ü�� ���͸� �̿��ؼ� ���� ���ϱ� >> �����ν� ��üȭ

        Vector3 vec1 = target.position;
        Vector3 vec2 = transform.parent.position;
        Vector3 vec3 = transform.position;



        float ang = Vector2.Angle(vec1, vec3);
        transform.rotation = Quaternion.Euler(0, 0, ang);// .Euler : ���� �� �ޱ�. �Ⱦ��� ���Ȱ�
        

        transform.position = vec2 + (vec1 - vec2).normalized * 0.76f; // ���� ��ǥ ������
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;//�˺�ֵ

    public float attackRange;//������Χ

    public float attackRate;//����Ƶ��

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<Character>()?.takeDamage(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;//ÉËº¦Öµ

    public float attackRange;//¹¥»÷·¶Î§

    public float attackRate;//¹¥»÷ÆµÂÊ

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<Character>()?.takeDamage(this);
    }
}

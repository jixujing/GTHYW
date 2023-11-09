using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocolateBuffet : MonoBehaviour, Interactable
{

    private bool isCanRecover = false;
    public void TriggerAction()
    {
        isCanRecover = true;
       
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isCanRecover==true)
        {
            collision.GetComponent<Character>()?.ChocolateBuffetrecover();
            Destroy(gameObject);
        }
        
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour, Interactable
{
    public GameObject boardText;
    private bool isStayBoard;
    public void TriggerAction()
    {
            boardText.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        isStayBoard = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isStayBoard = false;
        boardText.SetActive(false);
    }
}

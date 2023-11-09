using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour,Interactable
{
    private SpriteRenderer spriteRenderer;
    public Sprite TreasureOpened;
    public Sprite TreasureClosed;
    public bool isDone;
    public GameObject[] gos;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? TreasureOpened : TreasureClosed;
    }
    public void TriggerAction()
    {
        Debug.Log("open chest");
        if (!isDone)
        {
            OpenTreasure();
        }
    }

    private void OpenTreasure()
    {
        spriteRenderer.sprite = TreasureOpened;
        isDone = true;
        this.gameObject.tag = "Untagged";
        Vector3 pos = transform.position;
        Instantiate(gos[Random.Range(0, gos.Length)], pos, Quaternion.identity);
    }
}

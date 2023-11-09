using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour,Interactable
{
    [Header("广播")]
    public VoidEventSo saveDataEvent;

    [Header("变量参数")]
    public SpriteRenderer spriteRenderer;
    public Sprite darkSprite;
    public Sprite lightSprite;
    public bool isDone;



    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? lightSprite : darkSprite;
    }

    public void TriggerAction()
    {
        if (!isDone)
        {
            isDone = true;
            spriteRenderer.sprite = lightSprite;

            //TODO:保存数据
            saveDataEvent.RaiseEvent();

            this.gameObject.tag = "Untagged";
        }
    }
}

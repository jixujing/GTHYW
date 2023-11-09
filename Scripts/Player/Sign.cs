using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sign : MonoBehaviour
{
    private Animator anim;
    public GameObject signSprite;
    public bool canPress;
    public Transform playerTran;

    private Interactable targetItem;
    private PlayerInputControl playerInput;

    private void Awake()
    {
        // anim = GetComponentInChildren<Animator>();���ַ�����������ر�ʱ�޷����
        anim = signSprite.GetComponent<Animator>();
        playerInput = new PlayerInputControl();
        playerInput.Enable();
    }

    private void OnEnable()
    {
        playerInput.Gameplay.Confirm.started += OnConfirm;
    }

    private void OnDisable()
    {
        canPress = false;
    }
    private void OnConfirm(InputAction.CallbackContext obj)
    {
        if (canPress)
        {
            targetItem.TriggerAction();
            GetComponent<AudioDefination>()?.Raise();
        }
    }

    private void Update()
    {
        signSprite.SetActive(canPress);
        signSprite.transform.localScale = playerTran.localScale;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            canPress = true;
            targetItem = collision.GetComponent<Interactable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
            canPress = false;   
    }
}

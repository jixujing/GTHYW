using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Character : MonoBehaviour,ISaveable
{
    [Header("�¼�����")]
    public VoidEventSo newGameEvent;

    [Header("��������")]
    public float maxHealth;  //���Ѫ��
    public float currentHealth;//��ǰ����ֵ
    public float maxPower;
    public float currentPower;
    public float powerRecoverSpeed;


    [Header("�����޵�")]
    public float invulnerableDuration;//�޵�ʱ��
    [HideInInspector]public float invulnerableCount;//������
    public bool invulnerable;

    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent Ondie;
    public UnityEvent OnHealthChange;
    
    private void NewGame()
    {
        currentHealth = maxHealth;
        currentPower = maxPower;
        OnHealthChange?.Invoke();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    public void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();

    }


    private void Update()
    {
        if (invulnerable)
        {
            invulnerableCount -= Time.deltaTime;
            if(invulnerableCount <= 0)
            {
                invulnerable = false;
            }
        }

        if (currentPower < maxPower)
        {
            currentPower += Time.deltaTime * powerRecoverSpeed;

        }

        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.CompareTag("Water"))
        {
            if (currentHealth > 0)
            {
                currentHealth = 0;
                OnHealthChange?.Invoke();
                Ondie?.Invoke();
            }
        }
    }

    public void takeDamage(Attack attacker)
    {
        if (invulnerable)
            return;
        if(currentHealth -attacker.damage > 0)
        {
            currentHealth -= attacker.damage;
            TiggerInvulnerable();

            //ִ������
            OnTakeDamage?.Invoke(attacker.transform);
        }
        else
        {
            currentHealth = 0;

            //��������
            Ondie?.Invoke();


        }
        //Ѫ���仯 
        OnHealthChange?.Invoke();
    }

    public void ChocolateBuffetrecover()
    {
        currentHealth += 20;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        OnHealthChange?.Invoke();
    }

    public void Candy()
    {
        currentHealth += 10;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        OnHealthChange?.Invoke();
    }

    public void GalloPinto()
    {
        currentHealth += 30;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        OnHealthChange?.Invoke();
    }


    private void TiggerInvulnerable()
    {
        if (!invulnerable)
        {
            invulnerable = true;
            invulnerableCount = invulnerableDuration;
        }
    }

    public void onSlide(int cost)
    {
        currentPower -= cost;
        OnHealthChange.Invoke();
    }




    //�ӿ�
    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void GetSaveData(Data data)
    {
        if(data.characterPositionDict.ContainsKey(GetDataID().ID))
        {
            data.characterPositionDict[GetDataID().ID] = transform.position;
            data.floatSavedData[GetDataID().ID + "health"] = currentHealth;
            data.floatSavedData[GetDataID().ID + "power"] = currentPower;
        }
        else
        {
            data.characterPositionDict.Add(GetDataID().ID, transform.position);
            data.floatSavedData.Add(GetDataID().ID + "health", currentHealth);
            data.floatSavedData.Add(GetDataID().ID + "power", currentPower);
            
        }
    }

    public void LoadData(Data data)
    {
        if (data.characterPositionDict.ContainsKey(GetDataID().ID))
        {
            transform.position = data.characterPositionDict[GetDataID().ID];
            currentHealth = data.floatSavedData[GetDataID().ID + "health"];
            currentPower = data.floatSavedData[GetDataID().ID + "power"];

            //֪ͨUI����
            OnHealthChange?.Invoke();
        }
    }


}

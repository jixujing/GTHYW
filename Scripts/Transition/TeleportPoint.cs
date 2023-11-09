using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, Interactable
{
    public SceneLoadEventSO LoadEventSO;
    public GameSceneSO sceneToGo;
    public Vector3 positionToGo;
    public void TriggerAction()
    {
        Debug.Log("´«ËÍ");
        LoadEventSO.RaiseLoadRequestEvent(sceneToGo,positionToGo,true);
        
    }
}

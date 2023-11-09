using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]

public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneSO,Vector3,bool> LoadRequestEvent;
   
/// <summary>
/// 场景加载请求
/// </summary>
/// <param name="locationToLoad">要求加载的场景</param>
/// <param name="posToGo">player的目的坐标</param>
/// <param name="fadeScreen">是否渐入渐出</param>
    public void RaiseLoadRequestEvent(GameSceneSO locationToLoad , Vector3 posToGo, bool fadeScreen)
    {
        LoadRequestEvent?.Invoke(locationToLoad,posToGo,fadeScreen);
    } 
}
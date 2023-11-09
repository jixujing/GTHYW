using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Event/FadeEventSO")]
public class FadeEventSO : ScriptableObject
{
    public UnityAction<Color, float, bool> OnEventRaised;

    /// <summary>
    /// Öð½¥±äºÚ
    /// </summary>
    /// <param name="duration"></param>
    public void fadeIn(float duration)
    {
        RaiseEvent(Color.black,duration,true);
    }

    /// <summary>
    /// Öð½¥±äÍ¸Ã÷
    /// </summary>
    /// <param name="duration"></param>
    public void fadeOut(float duration)
    {
        RaiseEvent(Color.clear, duration, false);
    }

    private void RaiseEvent(Color target , float duration ,bool fadeIn)
    {
        OnEventRaised?.Invoke(target,duration,fadeIn);
    }
}
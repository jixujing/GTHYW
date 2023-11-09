using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Events/VoidEventSO")]
public class VoidEventSo : ScriptableObject
{
    public UnityAction OnEventRaised;

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}

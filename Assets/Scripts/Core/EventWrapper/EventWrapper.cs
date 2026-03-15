
using System;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EventWrapper<T> : EventWrapper
{
    public event Action<T> _onEvent = delegate { };
    [SerializeField] private UnityEvent<T> onUnityEvent;
    
    public void Raise(T arg)
    {
        base.Raise();
        _onEvent?.Invoke(arg);
        onUnityEvent?.Invoke(arg);
    }
}

[System.Serializable]
public class EventWrapper
{
    public event Action _onEventNoArgs = delegate { };
    [SerializeField] private UnityEvent onUnityEventNoArgs;
    
    public void Raise()
    {
        _onEventNoArgs?.Invoke();
        onUnityEventNoArgs?.Invoke();
    }
}
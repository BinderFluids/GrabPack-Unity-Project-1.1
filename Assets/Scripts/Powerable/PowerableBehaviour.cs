using System;
using UnityEngine;
using UnityEngine.Events;

public class PowerableBehaviour : MonoBehaviour, IPowerable
{
    public bool IsPowered { get; private set; }

    public event Action onPoweredOn;
    public event Action onPowerOff;
    public event Action<bool> onSetPowered;
    [SerializeField] private UnityEvent onPoweredOnUnityEvent;
    [SerializeField] private UnityEvent onPoweredOffUnityEvent;
    [SerializeField] private UnityEvent<bool> onSetPoweredUnityEvent;
    

    public void PowerOn()
    {
        IsPowered = true; 
        
        OnPoweredOn();
        OnPowered(true);
        
        onSetPowered?.Invoke(true);
        onSetPoweredUnityEvent?.Invoke(true);
        
        onPoweredOn?.Invoke();
        onPoweredOnUnityEvent?.Invoke();
    }
    
    public void PowerOff()
    {
        IsPowered = false;
        
        OnPoweredOff();
        OnPowered(false);
        
        onSetPowered?.Invoke(false);
        onSetPoweredUnityEvent?.Invoke(false);
        
        onPowerOff?.Invoke();
        onPoweredOffUnityEvent?.Invoke();
    }

    protected virtual void OnPowered(bool active) { } 
    protected virtual void OnPoweredOn() { }
    protected virtual void OnPoweredOff() { }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerableBehaviour : MonoBehaviour, IPowerable
{
    [field: SerializeField] public bool IsPowered { get; private set; }
    
    [Tooltip("Optional power source which will trigger this powerable's states")]
    [SerializeField] private List<InterfaceReference<IPowerable>> powerSourceReferences;

    public event Action onPoweredOn;
    public event Action onPowerOff;
    public event Action<bool> onSetPowered;
    [SerializeField] private UnityEvent onPoweredOnUnityEvent;
    [SerializeField] private UnityEvent onPoweredOffUnityEvent;
    [SerializeField] private UnityEvent<bool> onSetPoweredUnityEvent;

    private void Awake()
    {
        foreach (InterfaceReference<IPowerable> reference in powerSourceReferences)
            reference.Value.onPoweredOn += CheckAllPowerSourcesOn; 
    }

    private void Start()
    {
        SetPowered(IsPowered);
    }

    private void OnDestroy()
    {
        foreach (InterfaceReference<IPowerable> reference in powerSourceReferences)
            reference.Value.onPoweredOn -= CheckAllPowerSourcesOn;
    }

    private void CheckAllPowerSourcesOn()
    {
        if (powerSourceReferences.TrueForAll(reference => reference.Value.IsPowered))
        {
            if (IsPowered) return;
            PowerOn();    
        }
        else if (IsPowered)
            PowerOff();
        
    }

    public void SetPowered(bool active)
    {
        if (active)
            PowerOn();
        else
            PowerOff();
    }
    
    public void PowerOn()
    {
        if (IsPowered) return;
        
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
        if (!IsPowered) return;
        
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
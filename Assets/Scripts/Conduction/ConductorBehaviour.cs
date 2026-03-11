
using System;
using UnityEngine;
using UnityEngine.Events;

public interface IConductor
{
    event Action onConduct;
    void Conduct(ElementType elementType);
}

public class ConductorBehaviour : MonoBehaviour
{
    [SerializeField] private ElementType reactantType;
    public ElementType ReactantType => reactantType;

    [SerializeField] private UnityEvent onConductUnityEvent;
    public event Action onConduct = delegate {};
    
    public void Conduct(ElementType elementType)
    {
        if (elementType != this.reactantType) return;
        
        onConduct?.Invoke();
        onConductUnityEvent?.Invoke();
    }
}
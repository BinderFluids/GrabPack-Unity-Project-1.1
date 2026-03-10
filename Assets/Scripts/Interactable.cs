using System;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private bool canInteract = true;
    public bool CanInteract => canInteract;
    public void SetInteract(bool value) => canInteract = value;
    
    
    [SerializeField] private UnityEvent onInteractUnityEvent;
    public event Action onInteract = delegate {};

    public void Interact()
    {
        if (!canInteract) return;
        
        onInteract?.Invoke();
        onInteractUnityEvent?.Invoke();
    }
    protected virtual void OnInteract() { }
}
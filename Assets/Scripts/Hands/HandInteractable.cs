using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandInteractable : MonoBehaviour
{
    [SerializeField] protected List<BaseHandBehaviour> hands = new(); 
    [SerializeField] private UnityEvent onGrabUnityEvent;
    [SerializeField] private UnityEvent onReleaseUnityEvent;
    public event Action onGrab;
    public event Action onRelease;
    
    public void Grab(BaseHandBehaviour hand)
    {
        hands.Add(hand); 
        
        OnGrab(hand);
        onGrab?.Invoke();
        onGrabUnityEvent?.Invoke();
    }
    protected virtual void OnGrab(BaseHandBehaviour hand) { }

    public void Release(BaseHandBehaviour hand)
    {
        hands.Remove(hand); 
        
        OnRelease(hand);
        onRelease?.Invoke();
        onReleaseUnityEvent?.Invoke();
    }
    protected virtual void OnRelease(BaseHandBehaviour hand) { }
}

/*
 HandInteractable
    Grabbable: Hand sticks to the object
        Draggable: Has some sort of behaviour when the mouse is held down while being held
    Pickupable: Goes into the hand's inventory

*/
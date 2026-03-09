using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandInteractable : MonoBehaviour
{
    public enum GrabTypeEnum
    {
        Grip,
        None
    }

    [SerializeField] protected Transform _transform;
    
    [SerializeField] private GrabTypeEnum grabType = GrabTypeEnum.Grip;
    public GrabTypeEnum GrabType => grabType;
    
    [SerializeField] protected List<BaseHandBehaviour> hands = new(); 
    [SerializeField] private UnityEvent onGrabUnityEvent;
    [SerializeField] private UnityEvent onRetractUnityEvent;
    public event Action onGrab;
    public event Action onRetract;
    
    public void Grab(BaseHandBehaviour hand)
    {
        hands.Add(hand); 
        hand.SetParent(transform);
        
        OnGrab(hand);
        onGrab?.Invoke();
        onGrabUnityEvent?.Invoke();
    }
    protected virtual void OnGrab(BaseHandBehaviour hand) { }

    public void Retract(BaseHandBehaviour hand)
    {
        OnRetract(hand);
        onRetract?.Invoke();
        onRetractUnityEvent?.Invoke();
        hands.Remove(hand); 
    }
    protected virtual void OnRetract(BaseHandBehaviour hand) { }
}

/*
 HandInteractable
    Grabbable: Hand sticks to the object
        Draggable: Has some sort of behaviour when the mouse is held down while being held
    Pickupable: Goes into the hand's inventory

*/
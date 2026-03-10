using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HandInteractable : MonoBehaviour
{
    public enum GrabTypeEnum
    {
        Grip,
        None
    }
    
    [SerializeField] protected bool canInteract = true;
    public bool CanInteract => canInteract;
    public void SetInteractable(bool value) => canInteract = value;

    [SerializeField] protected Transform _transform;
    
    [SerializeField] private GrabTypeEnum grabType = GrabTypeEnum.Grip;
    public GrabTypeEnum GrabType => grabType;
    
    [SerializeField] protected List<BaseHandBehaviour> hands = new(); 
    public List<BaseHandBehaviour> Hands => hands;
    
    [SerializeField] private UnityEvent onGrabUnityEvent;
    [SerializeField] private UnityEvent onRetractUnityEvent;
    public event Action onGrab;
    public event Action onRetract;


    [SerializeField, Range(0, 2)] private int maxHands = 2;
    
    
    public bool Grab(BaseHandBehaviour hand)
    {
        if (hands.Count >= maxHands) return false;
        
        hands.Add(hand); 
        hand.SetParent(transform);
        
        if (!canInteract) return false;
        
        OnGrab(hand);
        onGrab?.Invoke();
        onGrabUnityEvent?.Invoke();

        return true;
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


    public void RetractAllHands()
    {
        List<BaseHandBehaviour> tempList = new List<BaseHandBehaviour>(hands);
        foreach (BaseHandBehaviour hand in tempList)
            hand.Retract();
    }


    public void UpdatePull(BaseHandBehaviour hand)
    {
        
    }
    protected virtual void OnUpdatePull(BaseHandBehaviour hand) {}
    public void LateUpdatePull(BaseHandBehaviour hand)
    {
        
    }

    protected virtual void OnLateUpdatePull(BaseHandBehaviour hand)
    {
        
    }
}

public class PressureHandInteractable : HandInteractable
{
    
}
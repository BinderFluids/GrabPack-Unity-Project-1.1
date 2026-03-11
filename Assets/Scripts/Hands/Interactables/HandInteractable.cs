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
    
    [Tooltip("Optional - If set, only interactable by this hand")]
    [SerializeField] private HandType targetHandType;
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
    public event Action<BaseHandBehaviour> onGrab;
    public event Action<BaseHandBehaviour> onRetract;


    [SerializeField, Range(1, 2)] private int maxHands = 2;
    
    
    public bool Grab(BaseHandBehaviour hand)
    {
        if (hands.Count >= maxHands) return false;
        if (!canInteract) return false;
        if (targetHandType != null && hand.HandType != targetHandType) return false;
        
        hands.Add(hand); 
        hand.SetParent(transform);
        
        OnGrab(hand);
        onGrab?.Invoke(hand);
        onGrabUnityEvent?.Invoke();

        return true;
    }
    protected virtual void OnGrab(BaseHandBehaviour hand) { }

    public void Retract(BaseHandBehaviour hand)
    {
        OnRetract(hand);
        onRetract?.Invoke(hand);
        onRetractUnityEvent?.Invoke();
        hands.Remove(hand); 
    }
    protected virtual void OnRetract(BaseHandBehaviour hand) { }


    public void RetractAllHands()
    {
        List<BaseHandBehaviour> tempList = new List<BaseHandBehaviour>(hands);
        foreach (BaseHandBehaviour hand in tempList)
            hand.Retract();
        hands.Clear();
    }


    public void UpdatePull(BaseHandBehaviour hand)
    {
        OnUpdatePull(hand);
    }
    protected virtual void OnUpdatePull(BaseHandBehaviour hand) {}
    public void LateUpdatePull(BaseHandBehaviour hand)
    {
        OnLateUpdatePull(hand); 
    }

    protected virtual void OnLateUpdatePull(BaseHandBehaviour hand)
    {
        
    }

    private void OnDestroy()
    {
        RetractAllHands();
    }
}

public class PressureHandInteractable : HandInteractable
{
    
}
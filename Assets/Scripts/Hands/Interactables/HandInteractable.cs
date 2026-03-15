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

    [Tooltip("Optional - If set, the hand will be attached to this transform")]
    [SerializeField] private Transform handAnchor; 
    
    [Tooltip("Optional - If set, only interactable by this hand")]
    [SerializeField] private HandType targetHandType;
    [SerializeField] protected bool canInteract = true;
    public bool CanInteract => canInteract;
    public void SetInteractable(bool value) => canInteract = value;

    [SerializeField] protected Transform _transform;
    
    [Tooltip("Defines which animation for the hand to play once interacted with")]
    [SerializeField] private GrabTypeEnum grabType = GrabTypeEnum.Grip;
    public GrabTypeEnum GrabType => grabType;
    
    private List<BaseHandBehaviour> hands = new(); 
    public List<BaseHandBehaviour> Hands => hands;
    public bool IsGrabbed => hands.Count > 0;
    
    [SerializeField] private UnityEvent onGrabUnityEvent;
    [SerializeField] private UnityEvent onRetractUnityEvent;
    public EventWrapper<BaseHandBehaviour> OnGrabWrapper = new();
    public EventWrapper<BaseHandBehaviour> OnRetractWrapper = new();
    public EventWrapper<GameObject> OnGrabGameObjectWrapper = new();
    public EventWrapper<GameObject> OnRetractGameObjectWrapper = new();


    [SerializeField, Range(1, 2)] private int maxHands = 2;

    private void Awake()
    {
        if (_transform == null) _transform = transform;
    }

    public bool Grab(BaseHandBehaviour hand)
    {
        if (hands.Count >= maxHands) return false;
        if (!canInteract) return false;
        if (targetHandType != null && hand.HandType != targetHandType) return false;
        
        hands.Add(hand);
        hand.SetParent(_transform, true);
        if (handAnchor != null) hand.transform.position = handAnchor.position;
        
        OnGrab(hand);
        OnGrabWrapper.Raise(hand);
        OnGrabGameObjectWrapper.Raise(hand.gameObject);

        return true;
    }
    protected virtual void OnGrab(BaseHandBehaviour hand) { }

    public void Retract(BaseHandBehaviour hand)
    {
        hands.Remove(hand); 
        
        OnRetract(hand);
        OnRetractWrapper.Raise(hand);
        OnRetractGameObjectWrapper.Raise(hand.gameObject);
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
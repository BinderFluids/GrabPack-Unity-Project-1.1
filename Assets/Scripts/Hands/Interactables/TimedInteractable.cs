using System;
using UnityEngine;
using UnityEngine.Events;

public class TimedInteractable : HandInteractable
{
    enum TimerType
    {
        Hold,
        Pull
    }
    [SerializeField] private TimerType timerType;
    
    [Tooltip("Optional - If set, only interactable by this hand")]
    [SerializeField] private BaseHandBehaviour targetHand;

    [SerializeField] private bool interactOnce = true;
    [SerializeField] private bool resetOnRelease; 
    [field: SerializeField] public float TimeToInteract { get; set; } = 1f;

    [SerializeField] private UnityEvent onInteractionTickEvent;
    public event Action onInteractionTick = delegate {};
    
    
    private float timeHeld = 0f;
    private bool isInteracting;
    
    private bool interactionFinished;
    public bool InteractionFinished => interactionFinished;

    [SerializeField] private UnityEvent onTimerFinishedEvent;
    public event Action onTimerFinished;
    
    protected override void OnGrab(BaseHandBehaviour hand)
    {
        if (targetHand != null)
            if (hand != targetHand) return;

        if (timerType == TimerType.Hold) isInteracting = true;
        if (resetOnRelease) timeHeld = 0f; 
    }

    private void Update()
    {
        if (timerType != TimerType.Hold) return;
        if (!isInteracting) return;
        IncrementTimer();
    }

    protected override void OnUpdatePull(BaseHandBehaviour hand)
    {
        if (timerType != TimerType.Pull) return;
        IncrementTimer();
    }

    void IncrementTimer()
    {
        if (interactionFinished) return;
        
        timeHeld += Time.deltaTime;
        
        onInteractionTickEvent?.Invoke();
        onInteractionTick?.Invoke();
        
        if (timeHeld >= TimeToInteract)
        {
            if (interactOnce) canInteract = false; 
            isInteracting = false;
            interactionFinished = true;
            onTimerFinished?.Invoke();
            onTimerFinishedEvent?.Invoke();
        }
    }
}
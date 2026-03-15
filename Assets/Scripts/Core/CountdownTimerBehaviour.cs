
using System;
using UnityEngine;
using ImprovedTimers;
using UnityEngine.Events;

public class CountdownTimerBehaviour : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float timerDuration = 10f;
    public float TimerDuration => timerDuration;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onTimerStartedUnityEvent;
    [SerializeField] private UnityEvent onTimerFinishedUnityEvent;
    
    private CountdownTimer timer;
    public CountdownTimer Timer => timer;

    
    
    private void Start()
    {
        timer = new CountdownTimer(timerDuration);
        timer.OnTimerStart += InvokeStartEvent;
        timer.OnTimerStop += InvokeFinishEvent;
    }

    private void OnDestroy()
    {
        timer.OnTimerStart -= InvokeStartEvent;
        timer.OnTimerStop -= InvokeFinishEvent;
    }

    private void InvokeStartEvent()
    {
        onTimerStartedUnityEvent?.Invoke();
    }
    private void InvokeFinishEvent()
    {
        onTimerFinishedUnityEvent?.Invoke();
    }
    
    public void StartTimer() =>
        timer.Start();

    public void StopTimer() =>
        timer.Stop();
}
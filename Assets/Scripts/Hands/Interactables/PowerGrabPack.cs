
using System;
using UnityEngine;
using UnityEngine.Rendering;

public class PowerGrabPack : MonoBehaviour
{
    [SerializeField] private HandInteractable handInteractable;

    private void Start()
    {
        handInteractable ??= GetComponent<HandInteractable>();
        handInteractable.onGrab += OnGrab;
        handInteractable.onRetract += OnRelease;
    }

    void OnGrab(BaseHandBehaviour hand)
    {
        hand.GrabPack.PowerableBehaviour.PowerOn();
    }

    void OnRelease(BaseHandBehaviour hand)
    {
        if (!handInteractable.IsGrabbed)
            hand.GrabPack.PowerableBehaviour.PowerOff();
    }

    private void OnDestroy()
    {
        handInteractable.onGrab -= OnGrab;
        handInteractable.onRetract -= OnRelease;
    }
}
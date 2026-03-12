
using System;
using UnityEngine;
using UnityEngine.Rendering;

public class PowerGrabPack : MonoBehaviour
{
    [SerializeField] private HandInteractable handInteractable;

    private void Start()
    {
        handInteractable ??= GetComponent<HandInteractable>();
        handInteractable.OnGrabWrapper._onEvent += OnGrab;
        handInteractable.OnRetractWrapper._onEvent += OnRelease;
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
        handInteractable.OnGrabWrapper._onEvent -= OnGrab;
        handInteractable.OnRetractWrapper._onEvent -= OnRelease;
    }
}
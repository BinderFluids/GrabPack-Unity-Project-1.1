using System;
using UnityEngine;

public class Grabbable : HandInteractable
{
    [SerializeField] private Transform _transform;

    private void Start()
    {
        _transform ??= transform;
    }

    protected override void OnGrab(BaseHandBehaviour hand)
    {
        hand.SetParent(_transform); 
    }
}
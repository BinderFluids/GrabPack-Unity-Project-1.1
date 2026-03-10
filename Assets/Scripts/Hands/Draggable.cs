using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Draggable : HandInteractable
{
    [SerializeField] private Rigidbody rb;
    public Rigidbody Rigidbody => rb;

    private void Start()
    {
        rb ??= GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        if (hands.Count < 1) return;
        foreach (BaseHandBehaviour hand in hands)
        {
            if (!hand.MouseButtonHeld) return;
            
            Vector3 targetPos = hand.Origin.position;
            
            Vector3 direction = targetPos - rb.position;

            Vector3 dirNormalized = direction.normalized;

            float constantPullForce = hand.PullSpeed * 350; // increase this for a stronger pull
            float damping = 8f;

            Vector3 force =
                dirNormalized * constantPullForce
                - rb.linearVelocity * damping;

            rb.AddForce(force, ForceMode.Force);
        }
    }
}
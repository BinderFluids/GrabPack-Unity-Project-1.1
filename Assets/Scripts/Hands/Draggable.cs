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
            Debug.Log($"{hand.gameObject.name} is dragging {gameObject.name}");
            
            Vector3 targetPos = hand.handOrigin.position;
            
            Vector3 direction = targetPos - rb.position;
            float distance = direction.magnitude;

            Vector3 dirNormalized = direction.normalized;

            float constantPullForce = hand.pullSpeed * 350; // increase this for a stronger pull
            float damping = 8f;

            Vector3 force =
                dirNormalized * constantPullForce
                - rb.linearVelocity * damping;

            rb.AddForce(force, ForceMode.Force);
        }
    }
    
    
}
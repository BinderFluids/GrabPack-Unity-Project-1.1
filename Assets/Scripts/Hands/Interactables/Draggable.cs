using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Draggable : HandInteractable, IPressureHandInteractable
{
    [SerializeField] private Rigidbody rb;
    public Rigidbody Rigidbody => rb;

    private void Start()
    {
        rb ??= GetComponent<Rigidbody>();
    }

    public void ReleasePressure(BaseHandBehaviour hand, float pressure)
    {
        Vector3 launchDir =(_transform.position - hand.Origin.position).normalized;
        float launchForce = pressure * 100f;
        
        //handgrabbing.SetBool("grabbing", true);
        rb.isKinematic = false;
        rb.AddForce(launchDir * launchForce, ForceMode.Impulse);
    }

    protected override void OnLateUpdatePull(BaseHandBehaviour hand)
    {
        Vector3 targetPos = hand.Origin.position;
        Vector3 direction = targetPos - rb.position;
        Vector3 dirNormalized = direction.normalized;

        float constantPullForce = hand.GrabPack.PullSpeed * 350; 
        float damping = 8f;

        Vector3 force =
            dirNormalized * constantPullForce
            - rb.linearVelocity * damping;

        rb.AddForce(force, ForceMode.Force);
    }
}
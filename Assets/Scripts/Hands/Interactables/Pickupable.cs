using UnityEngine;
using UnityEngine.Events;

public class Pickupable : HandInteractable
{
    [SerializeField] private Rigidbody rb;
    public Rigidbody Rigidbody => rb;
    [SerializeField] private BoxCollider col;

    [SerializeField] private UnityEvent<Pickupable> onDrop; 
    
    protected override void OnGrab(BaseHandBehaviour hand)
    {
        hand.GiveItem(this); 
    }

    public void Drop(BaseHandBehaviour hand)
    {
        _transform.SetParent(null, true); 
        rb.isKinematic = false;
        col.enabled = true;
        onDrop.Invoke(this);
    }

    protected override void OnRetract(BaseHandBehaviour hand)
    {
        if (rb != null)
        {
            rb.isKinematic = true;
            col.enabled = false;

            _transform.SetParent(hand.Transform, true);
        }
    }
}
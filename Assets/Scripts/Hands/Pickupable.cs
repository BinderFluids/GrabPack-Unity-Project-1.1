using UnityEngine;
using UnityEngine.XR;

public class Pickupable : HandInteractable
{
    [SerializeField] private Rigidbody rb;
    public Rigidbody Rigidbody => rb;
    [SerializeField] private BoxCollider col;
    
    protected override void OnGrab(BaseHandBehaviour hand)
    {
        hand.GiveItem(this); 
    }

    public void Drop(BaseHandBehaviour hand)
    {
        Debug.Log($"Dropped it {gameObject.name}");
        _transform.SetParent(null, true); 
        rb.isKinematic = false;

        // if (Hand == "Left")
        // {
            //rb.AddForce(-_transform.up * 800f, ForceMode.Impulse);
            // aimOverride.leftActive = false;
        // }
        // if (Hand == "Right")
        // {
        //     batteryRB.AddForce(transform.up * 800f, ForceMode.Impulse);
        //     aimOverride.rightActive = false;
        // }

        //CableSim.isActive = false;

        col.enabled = true;
        //battery = null;
        //handgrabbing.SetBool("grabbing", false);

        //return;
    }

    protected override void OnRetract(BaseHandBehaviour hand)
    {
        if (rb != null)
        {
            rb.isKinematic = true;
            col.enabled = false;

            _transform.SetParent(hand.transform, true);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupableSocket : MonoBehaviour
{

    public LayerMask layerMask;

    [Tooltip("Optional - The GameObject that will be attached to this socket")] [SerializeField]
    private Pickupable targetItem;

    [SerializeField] private Pickupable containedItem;
    [SerializeField] private Transform anchor;
    public bool HasItem => containedItem != null; 
    
    [SerializeField] private UnityEvent<bool> onItemChangedUnityEvent;
    public event Action<bool> onItemChanged = delegate {};


    public void GiveItem(Collider collider)
    {
        if (HasItem) return;
        if (!collider.TryGetComponent(out Pickupable pickupable)) return;
        
        
        //Get specified item
        if (targetItem != null && pickupable == targetItem)
            ContainItem(pickupable);
        //Get item on layer
        else if ((layerMask.value & (1 << pickupable.gameObject.layer)) != 0)
            ContainItem(pickupable);
    }

    void ContainItem(Pickupable pickupable)
    {
        containedItem = pickupable;
        containedItem.transform.SetParent(anchor);
        containedItem.Rigidbody.isKinematic = true;
        containedItem.transform.position = anchor.position;
        containedItem.transform.rotation = anchor.rotation;
        containedItem.SetSocket(this);
        
        onItemChangedUnityEvent?.Invoke(true);
        onItemChanged?.Invoke(true);
    }

    public void RemoveItem()
    {
        if (containedItem == null) return;
        containedItem.transform.SetParent(null);
        containedItem.SetSocket(null); 
        containedItem = null;
        
        onItemChangedUnityEvent?.Invoke(false);
        onItemChanged?.Invoke(false);
    }
    
    // public void OnTriggerStay(Collider other)
    // {
    //
    //     if (full == false)
    //     {
    //         if ((layerMask & (1 << other.gameObject.layer)) != 0 && other.gameObject.name != "Gear")
    //         {
    //
    //
    //             Rigidbody rb = other.GetComponent<Rigidbody>();
    //             rb.isKinematic = true;
    //             other.gameObject.transform.position = anchor.transform.position;
    //             other.gameObject.transform.rotation = anchor.transform.rotation;
    //             full = true;
    //
    //             batterycollider = other;
    //         }
    //     }
    //
    //
    //
    // }
    //
    // public void OnTriggerExit(Collider other)
    // {
    //
    //     if ((layerMask & (1 << other.gameObject.layer)) != 0)
    //     {
    //
    //         full = false;
    //     }
    //
    // }
    //
    // void LateUpdate()
    // {
    //     try
    //     {
    //         if (batterycollider.gameObject.transform.position != anchor.transform.position)
    //         {
    //             full = false;
    //             batterycollider = null;
    //         }
    //     }
    //     catch (UnassignedReferenceException e)
    //     {
    //         
    //     }
    //     
    // }
}

using System;
using UnityEngine;

public class HandPickUp : MonoBehaviour, IInteractable
{
    [SerializeField] private HandsInventory inventory;
    public RigidboyPlayerController player;

    public HandType handToGive;
    public AudioClip pickupsx;


    public event Action onInteract;
    public void Interact()
    {
        if (!inventory.TryAdd(handToGive)) return;
        
        GlobalAudio.Instance.PlayOneShot(pickupsx);
        onInteract?.Invoke();
        gameObject.SetActive(false);
    }
}
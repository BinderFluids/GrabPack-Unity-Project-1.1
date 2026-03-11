using System;
using UnityEngine;

public class HandPickUp : MonoBehaviour, IInteractable
{
    [SerializeField] private HandsInventory inventory;
    public RigidboyPlayerController player;

    public HandType handToGive;

    public AudioSource globalaudio;
    public AudioClip pickupsx;


    public event Action onInteract;
    public void Interact()
    {
        if (!inventory.TryAdd(handToGive)) return;
        
        globalaudio.PlayOneShot(pickupsx);
        onInteract?.Invoke();
        gameObject.SetActive(false);
    }
}
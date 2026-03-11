
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField] private GrabPackManager grabPack;
    [SerializeField] private Camera cam;

    [Header("Settings")]
    [SerializeField] private CablePhysics physics; 
    [SerializeField] private RotateArm rotateArm;
    [SerializeField] private int handNormal; 
    [SerializeField] private int mouseIndex;   
    
    [Header("Hand")] 
    [SerializeField] private BaseHandBehaviour hand;
    public BaseHandBehaviour Hand => hand;
    
    private void Start()
    {
        cam ??= Camera.main;
        if (hand != null) EnableHand(hand); 
    }

    void SubscribeToHandEvents()
    {
        hand.onFire += OnFire;
        hand.onRetract += OnRetract;
    }
    void UnsubscribeFromHandEvents()
    {
        hand.onFire -= OnFire;
        hand.onRetract -= OnRetract;
    }

    private void Update()
    {
        HandleHandInput();
    }

    private void LateUpdate()
    {
        LateHandleInput(); 
    }
    

    void HandleHandInput()
    {
        hand?.HandleInput(mouseIndex, CastRay(), handNormal);
    }
    void LateHandleInput()
    {
        hand?.LateHandleInput(mouseIndex, CastRay(), handNormal);
    }

    void OnFire(BaseHandBehaviour hand)
    {
        rotateArm.SetActive(true, hand.TargetPoint);
    }
    void OnRetract(BaseHandBehaviour hand)
    {
        rotateArm.SetActive(false, Vector3.zero);
    }

    public void DisableHand()
    {
        if (hand == null) return; 
        
        UnsubscribeFromHandEvents();
        
        hand.DisableHand();
        hand = null; 
    }
    public void EnableHand(BaseHandBehaviour hand)
    {
        if (hand == null)
        {
            Debug.LogError("Trying to enable a Hand that is null");
            return;
        }
        
        this.hand = hand; 
        hand.EnableHand(grabPack, physics);
        
        SubscribeToHandEvents(); 
    }
    
    Ray CastRay()
    {
        Vector3 center = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
        return cam.ScreenPointToRay(center);
    }

    private void OnDestroy()
    {
        hand.onFire -= OnFire;
        hand.onRetract -= OnRetract;
    }
}
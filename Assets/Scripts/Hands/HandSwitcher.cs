
using System;
using UnityEngine;

public class HandSwitcher : MonoBehaviour
{
    [SerializeField] private Animator grabPackAnimator;
    [SerializeField] private HandController handController;
    [SerializeField] private HandsInventory inventory;

    private BaseHandBehaviour handToEnable;
    private bool isSwitching; 

    private void Update()
    {
        HandleInventoryInput();
    }

    void HandleInventoryInput()
    {
        if (isSwitching) return;
        
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i) ||
                Input.GetKeyDown(KeyCode.Keypad0 + i))
            {
                BaseHandBehaviour newHand = inventory.GetHand(i);
                if (newHand == null) return;
                if (handController.Hand == newHand) return; 
                
                isSwitching = true;
                handToEnable = newHand;
                
                grabPackAnimator.SetBool("switch", true);
                grabPackAnimator.SetTrigger("Switch");
            }
        }
    }

    
    //Called from animation events
    public void SwitchHand()
    {
        handController.DisableHand();
        handController.EnableHand(handToEnable);
    }
    public void OnSwitchEnd()
    {
        isSwitching = false; 
    }
}
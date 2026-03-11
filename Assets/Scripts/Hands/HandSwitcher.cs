
using System;
using UnityEngine;

public class HandSwitcher : MonoBehaviour
{
    [SerializeField] private Animator grabPackAnimator;
    [SerializeField] private HandsController handsController;
    [SerializeField] private HandsInventory inventory;

    private int handToEnable;
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
                if (handsController.HandConfigs[1].hand == newHand) return; 
                
                grabPackAnimator.SetBool("switch", true);
                grabPackAnimator.SetTrigger("Switch");

                handToEnable = i;
                isSwitching = true;
            }
        }
    }

    
    //Called from animation events
    public void SwitchHand()
    {
        BaseHandBehaviour newHand = inventory.GetHand(handToEnable);
        handsController.DisableHand(1);
        handsController.EnableHand(1, newHand);
    }
    public void OnSwitchEnd()
    {
        isSwitching = false; 
    }
}
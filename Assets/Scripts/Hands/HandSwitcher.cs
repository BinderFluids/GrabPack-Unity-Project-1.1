
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
        for (int i = 0; i <= 9; i++)
            if (Input.GetKeyDown(KeyCode.Alpha0 + i + 1) ||
                Input.GetKeyDown(KeyCode.Keypad0 + i + 1))
                SwitchHand(i);
    }

    public void SwitchHand(BaseHandBehaviour newHand)
    {
        if (isSwitching) return;
        
        if (newHand == null) return;
        if (handController.Hand == newHand) return; 
        
        isSwitching = true;
        handToEnable = newHand;
                
        handController.Hand?.ReleaseItem();
        grabPackAnimator.SetBool("switch", true);
        grabPackAnimator.SetTrigger("Switch");
    }
    public void SwitchHand(int i)
    {
        SwitchHand(inventory.GetHand(i));
    }
    
    //Called from animation events
    void SwitchHandOnAnimation()
    {
        handController.DisableHand();
        handController.EnableHand(handToEnable);
    }
    void OnSwitchEnd()
    {
        isSwitching = false; 
    }
}
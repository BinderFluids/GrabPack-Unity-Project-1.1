using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ElectricalReciever : BasePowerable
{
    public List<BasePowerable> requiredPowerables;
    private bool allPowered => requiredPowerables.All(p => p.IsPowered); 
    
    public AudioSource GlobalAudio;
    public AudioClip puzzlecomplete;

    public GameObject[] hands;

    private void Start()
    {
        foreach (BasePowerable powerable in requiredPowerables)
            powerable.onPoweredOn += CheckAllPoweredOn; 
    }
    
    void CheckAllPoweredOn()
    {
        if (allPowered)
            CompleteCircuit(); 
    }
    

    public void CompleteCircuit()
    {
        foreach (GameObject hand in hands)
        {
            if (hand != null && hand.activeInHierarchy && hand.transform.parent == transform)
            {
                PowerOn();
                
                GlobalAudio.PlayOneShot(puzzlecomplete, 1.0f);
                ReturnAllHands();
            }
        }
    }


    void ReturnAllHands()
    {
        LaunchHand[] hands = FindObjectsOfType<LaunchHand>();

        foreach (LaunchHand hand in hands)
        {
            hand.return1();
        }
    }
    
    private void OnDestroy()
    {
        foreach (BasePowerable powerable in requiredPowerables)
            if (powerable != null)
                powerable.onPoweredOn -= CheckAllPoweredOn;
    }
}

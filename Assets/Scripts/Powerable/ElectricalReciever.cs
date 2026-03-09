using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ElectricalReciever : PowerableBehaviour
{
    [SerializeField] private List<InterfaceReference<IPowerable>> requiredPowerables;
    [SerializeField] private List<InterfaceReference<IPowerable>> powerablesOnComplete = new();
    private bool allPowered => 
        requiredPowerables.All(powerable => powerable.Value.IsPowered);
    
    public AudioSource GlobalAudio;
    public AudioClip puzzlecomplete;

    public GameObject[] hands;

    private void Start()
    {
        foreach (PowerableBehaviour powerable in requiredPowerables)
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
                powerablesOnComplete.
                    ForEach(powerable => powerable.Value.PowerOn());
                
                GlobalAudio.PlayOneShot(puzzlecomplete, 1.0f);
                ReturnAllHands();
            }
        }
    }

    void ReturnAllHands()
    {
        BaseHandBehaviour[] hands = FindObjectsOfType<BaseHandBehaviour>();

        foreach (BaseHandBehaviour hand in hands)
        {
            hand.Return();
        }
    }
    
    private void OnDestroy()
    {
        foreach (PowerableBehaviour powerable in requiredPowerables)
            if (powerable != null)
                powerable.onPoweredOn -= CheckAllPoweredOn;
    }
}

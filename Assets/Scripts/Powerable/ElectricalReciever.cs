using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElectricalReciever : PowerableBehaviour
{
    [SerializeField] private HandInteractable handInteractable; 
    [SerializeField] private List<InterfaceReference<IPowerable>> requiredPowerables;
    [SerializeField] private List<InterfaceReference<IPowerable>> powerablesOnComplete = new();
    private bool allPowered => 
        requiredPowerables.All(powerable => powerable.Value.IsPowered);
    
    public AudioSource GlobalAudio;
    public AudioClip puzzlecomplete;
    
    private void Start()
    {
        foreach (PowerableBehaviour powerable in requiredPowerables)
            powerable.onPoweredOn += CheckAllPoweredOn; 
    }
    
    public void CheckAllPoweredOn()
    {
        if (handInteractable.Hands.Count == 0) return;
        
        if (allPowered)
            CompleteCircuit(); 
    }

    public void CompleteCircuit()
    {
        PowerOn();
        powerablesOnComplete.
            ForEach(powerable => powerable.Value.PowerOn());
            
        GlobalAudio.PlayOneShot(puzzlecomplete, 1.0f);
        handInteractable.RetractAllHands();
    }
    
    private void OnDestroy()
    {
        foreach (PowerableBehaviour powerable in requiredPowerables)
            if (powerable != null)
                powerable.onPoweredOn -= CheckAllPoweredOn;
    }
}

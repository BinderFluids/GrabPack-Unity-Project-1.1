using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScanner : MonoBehaviour
{
    [SerializeField] private TimedInteractable timedInteractable;



    [Header("Renderers")] 
    [SerializeField] private MeshRenderer scanningLabel;
    [SerializeField] private MeshRenderer handPrint;


    [Header("Materials")] 
    [SerializeField] private Material ready;
    [SerializeField] private Material scanning; 
    [SerializeField] private Material verified;
    [SerializeField] private Material smile; 

    [Header("Colors")]
    [SerializeField] private Light scannerLight;
    [SerializeField] private MaterialColorSetter colorSetter;
    
    [SerializeField] private Color scannercolour;
    [ColorUsage(true, true)]
    [SerializeField] private Color scannerEmission;

    [SerializeField] private Color greenLightColor;
    [ColorUsage(true, true)]
    [SerializeField] private Color greenLightEmission;

    [SerializeField] private GameObject scanningAudio;
    private bool finishedScanning; 

    // Start is called before the first frame update
    void Awake()
    {
        ApplyColors(scannercolour, scannerEmission);

        timedInteractable.OnGrabWrapper._onEventNoArgs += OnGrab;
        timedInteractable.OnRetractWrapper._onEventNoArgs += OnRetract; 
        timedInteractable.onTimerFinished += OnInteractionFinished;
    }

    void ApplyColors(Color color, Color emission)
    {
        scannerLight.color = color;
        colorSetter.SetColor(color);
        colorSetter.SetEmissionColor(emission);
    }

    void OnGrab()
    {
        if (!finishedScanning)
        {
            scanningLabel.material = scanning;
            scanningAudio.SetActive(true);
        }    
    }

    void OnRetract()
    {
        scanningAudio.SetActive(false);
        
        if (!finishedScanning)
        {
            scanningLabel.material = ready;
        }
    }

    void OnInteractionFinished()
    {
        finishedScanning = true;
        ApplyColors(greenLightColor, greenLightEmission);
        
        scanningAudio.SetActive(false);
        scanningLabel.material = verified;
        handPrint.material = smile;
    }

    private void OnDestroy()
    {
        timedInteractable.OnGrabWrapper._onEventNoArgs -= OnGrab;
        timedInteractable.OnRetractWrapper._onEventNoArgs -= OnRetract; 
        timedInteractable.onTimerFinished -= OnInteractionFinished;
    }
    
}
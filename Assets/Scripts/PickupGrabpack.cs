using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGrabpack : MonoBehaviour, IInteractable
{
    public bool hasRedHand = true;
    public bool hasBlueHand = true;
    public bool hasPurpleHand = true;
    public bool hasPressureHand = true;
    public bool hasConductiveHand = true;


    // public GameObject RedHand;
    // public GameObject PurpleHand;
    // public GameObject FlareHand;
    // public GameObject conductiveHand;
    // public GameObject BlueHand;


    public GameObject MockRedHand;
    public GameObject MockPurpleHand;
    public GameObject MockFlareHand;
    public GameObject MockconductiveHand;
    public GameObject MockBlueHand;

    public HandManager handmanager;

    public event Action onInteract; 
    public void Interact()
    {
        gameObject.SetActive(false);
        handmanager.hasGrabPack = true;
    }

    void Start()
    {
        if (handmanager.hasGrabPack == true)
        {
            gameObject.SetActive(false);
        }


        if (hasBlueHand)
        {
            MockBlueHand.SetActive(true);
        }


        if (hasRedHand)
            MockRedHand.SetActive(true);
        else if (hasPurpleHand)
            MockPurpleHand.SetActive(true);
        else if (hasPressureHand)
            MockFlareHand.SetActive(true);
        else if (hasConductiveHand)
            MockconductiveHand.SetActive(true);
    }
}
